using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.UserRequests
{
    public class UserEditRequestDTO : UserBaseRequestDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "FirstName must be between 1 and 20 symbols!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "LastName must be between 1 and 20 symbols!")]
        public string LastName { get; set; }
    }
}
