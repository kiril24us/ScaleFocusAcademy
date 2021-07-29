using System.ComponentModel.DataAnnotations;

namespace ToDoApplication.DTO.DTO.Requests.UserRequests
{
    public class UserCreateRequestDTO : UserBaseRequestDTO
    {        
        [Required]
        [ValidationForRole(ErrorMessage = "You must enter 1 for Admin Role or 2 for Regular User Role")]
        public int Role { get; set; }
    }

    public class ValidationForRoleAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int number;
            bool isSuccess = int.TryParse(value.ToString(), out number);

            if(isSuccess)
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
