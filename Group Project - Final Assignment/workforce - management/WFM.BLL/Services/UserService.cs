using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

using WFM.DAL.Entities;
using WFM.BLL.Interfaces;
using WFM.DAL.Repositories.Interfaces;

namespace WFM.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole<Guid>> _roleMngr;
        private readonly IDaysOffLimitDefaultService _daysOffLimitDefaultService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITimeOffRequestService _timeOffRequestService;

        public UserService(IUserRepository userRepository, RoleManager<IdentityRole<Guid>> roleMngr, IDaysOffLimitDefaultService daysOffLimitDefaultService, IDateTimeProvider dateTimeProvider, ITimeOffRequestService timeOffRequestService)
        {
            _userRepository = userRepository;
            _roleMngr = roleMngr;
            _daysOffLimitDefaultService = daysOffLimitDefaultService;
            _dateTimeProvider = dateTimeProvider;
            _timeOffRequestService = timeOffRequestService;
        }

        public async Task<bool> CreateAsync(User user, string password, string roleName, Guid creatorId)
        {
            if (!await CheckIfRoleExists(roleName) || !await CheckIfCountryOfResidenceExists(user.CountryOfResidence))
            {
                return false;
            }

            if (await _userRepository.FindByName(user.UserName) != null)
            {
                return false;
            }

            if (await _userRepository.FindByEmail(user.Email) != null)
            {
                return false;
            }
            
            DaysOffLimitDefault daysOffDefaultLimits = await _daysOffLimitDefaultService.FindByCountryCode(user.CountryOfResidence);

            await CalculateDaysOffForNewUser(user, daysOffDefaultLimits);

            var dateTimeNow = _dateTimeProvider.UtcNow;
            user.CreatedOn = dateTimeNow;
            user.ModifiedOn = dateTimeNow;
            user.CreatedBy = creatorId;
            user.ModifiedBy = creatorId;

            return await _userRepository.Create(user, password);
        }

        public async Task<bool> DeleteAsync(User user)
        {
            int paidDaysOffToReturn = await _timeOffRequestService.CancelUserTimeOffRequests(user.Id);

            user.AvailablePaidDaysOff += paidDaysOffToReturn;
            user.IsDeleted = true;
            user.DeletedOn = _dateTimeProvider.UtcNow;
            user.FirstName = null;
            user.LastName = null;
            user.CountryOfResidence = null;
            user.UserName = user.Id.ToString();
            user.Email = null;

            // send emails to all team members
            // set crone job (delete time off request after 6 months)


            return await _userRepository.Delete(user);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepository.FindById(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetByNameAsync(string userName)
        {
            return await _userRepository.FindByName(userName);
        }

        public async Task<bool> CheckIfRoleExists(string roleName)
        {
            return await _roleMngr.RoleExistsAsync(roleName);
        }

        public async Task<bool> CheckIfCountryOfResidenceExists(string countryCode)
        {
            if (await _daysOffLimitDefaultService.FindByCountryCode(countryCode) == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> AddUserToRole(User user, string roleName)
        {
            if (!await _roleMngr.RoleExistsAsync(roleName))
            {
                return false;
            }

            return await _userRepository.AddUserToRole(user, roleName);
        }

        public async Task UpdateAsync(User user, Guid modifier)
        {
            user.ModifiedOn = _dateTimeProvider.UtcNow;
            user.ModifiedBy = modifier;
            await _userRepository.Update(user);
        }

        public async Task<bool> IsUserInRole(User user, string roleName)
        {
            return await _userRepository.IsUserInRole(user, roleName);
        }

        private Task CalculateDaysOffForNewUser(User user, DaysOffLimitDefault daysOffDefaultLimits)
        {
            int currentMonth = _dateTimeProvider.CurrentMonth;
            double daysOffCoefficient = 12.0 / currentMonth;

            user.AvailablePaidDaysOff = daysOffDefaultLimits.PaidDaysOff + 1 - (int)(daysOffDefaultLimits.PaidDaysOff / daysOffCoefficient);
            user.AvailableUnpaidDaysOff = daysOffDefaultLimits.UnpaidDaysOff + 1 - (int)(daysOffDefaultLimits.UnpaidDaysOff / daysOffCoefficient);
            user.AvailableSickLeaveDaysOff = daysOffDefaultLimits.SickLeaveDaysOff;

            return Task.CompletedTask;
        }
    }
}
