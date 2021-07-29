using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.TeamRequests
{
    public class AssignUserToATeamRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }
    }
}
