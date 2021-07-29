using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.ProjectRequests
{
    public class AssignProjectToATeamRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TeamId { get; set; }
    }
}
