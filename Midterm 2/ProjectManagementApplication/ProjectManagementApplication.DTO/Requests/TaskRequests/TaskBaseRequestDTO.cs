using ProjectManagementApplication.DTO.Requests.UserRequests;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.TaskRequests
{
    public abstract class TaskBaseRequestDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [ValidationForEnum(ErrorMessage = "You must enter 1 for Completed Task or 2 for Pending Task")]
        public int Status { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }
    }
}
