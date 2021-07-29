using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.UserRequests
{
    public class UserCreateRequestDTO : UserBaseRequestDTO, IValidatableObject
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required2]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 20 symbols!")]
        public string Password { get; set; }

        [Required2]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 20 symbols!")]
        public string RepeatPassword { get; set; }

        [Required]
        [ValidationForEnum(ErrorMessage = "You must enter 1 for Admin Role or 2 for Regular User Role")]
        public int Role { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (Password != RepeatPassword)
            {
                result.Add(new ValidationResult("Passwords do not match", new string[] { "Password" }));
            }
            return result;
        }        
    }

    public class Required2Attribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
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
