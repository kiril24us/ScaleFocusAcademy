using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.TaskRequests
{
    public class TaskEditRequestDTO : TaskBaseRequestDTO
    {
        [Required]
        public string AssigneeId { get; set; }
    }
}
