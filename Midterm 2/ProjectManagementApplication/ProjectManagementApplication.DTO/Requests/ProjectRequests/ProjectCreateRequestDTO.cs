using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.DTO.Requests.ProjectRequests
{
    public class ProjectCreateRequestDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 symbols!")]
        public string Name { get; set; }
    }
}
