using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.UserRequests
{
    public class UserCreateRequestDTO : UserBaseRequestDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [ValidationForEnum(ErrorMessage = "You must enter 1 for Admin Role or 2 for Regular User Role")]
        public int Role { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }
    }

    public class ValidationForEnumAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int number;
            bool isSuccess = int.TryParse(value.ToString(), out number);

            if (isSuccess)
            {
                if (number != 1 && number != 2)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
