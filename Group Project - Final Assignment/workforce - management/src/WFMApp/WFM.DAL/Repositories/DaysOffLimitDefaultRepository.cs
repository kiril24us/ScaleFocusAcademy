using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using WFM.DAL.Context;
using WFM.DAL.Entities;
using WFM.DAL.Repositories.Interfaces;
using WFM.DAL.Repositories.BaseGenericRepository;

namespace WFM.DAL.Repositories
{
    public class DaysOffLimitDefaultRepository : Repository<DaysOffLimitDefault>, IDaysOffLimitDefaultRepository
    {

        public DaysOffLimitDefaultRepository(AppEntityContext appEntityContext) : base(appEntityContext)
        {

        }

        public async Task<DaysOffLimitDefault> CreateDaysOffLimitDefaultAsync(DaysOffLimitDefault daysOffLimitDefault)
        {

            return await AddAsync(daysOffLimitDefault);
        }

        public async Task<DaysOffLimitDefault> DeleteDaysOffLimitDefaultAsync(DaysOffLimitDefault daysOffLimitDefault)
        {
            try
            {
                if (daysOffLimitDefault == null)
                {
                    throw new ArgumentNullException($"{nameof(DeleteDaysOffLimitDefaultAsync)} entity must not be null");
                }

                DbContext.Remove(daysOffLimitDefault);
                await DbContext.SaveChangesAsync();

                return daysOffLimitDefault;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DaysOffLimitDefault> FindByCountryCodeAsync(string countryCode)
        {
            try
            {
                return await GetAll().FirstOrDefaultAsync(t => t.CountryCode == countryCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ICollection<DaysOffLimitDefault>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<DaysOffLimitDefault> UpdateDaysOffLimitDefaultAsync(DaysOffLimitDefault daysOffLimitDefault)
        {
            return await UpdateAsync(daysOffLimitDefault);

        }
    }
}
