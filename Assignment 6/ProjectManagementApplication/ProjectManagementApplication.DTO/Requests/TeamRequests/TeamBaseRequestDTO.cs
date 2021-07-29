using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.TeamRequests
{
    public class TeamBaseRequestDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Team name must be between 1 and 20 symbols!")]
        public string Name { get; set; }
    }
}
