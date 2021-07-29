using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.WorkLogRequests
{
    public class WorkLogDeleteRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TaskId { get; set; }
    }
}
