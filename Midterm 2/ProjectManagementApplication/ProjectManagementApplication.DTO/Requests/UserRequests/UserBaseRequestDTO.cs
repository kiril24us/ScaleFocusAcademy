using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.UserRequests
{
    public abstract class UserBaseRequestDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Username must be between 1 and 10 symbols!")]
        public string Username { get; set; }
    }
}
