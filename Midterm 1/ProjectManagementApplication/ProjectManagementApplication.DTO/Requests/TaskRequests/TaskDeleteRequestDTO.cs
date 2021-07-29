using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.TaskRequests
{
    public class TaskDeleteRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }
    }
}
