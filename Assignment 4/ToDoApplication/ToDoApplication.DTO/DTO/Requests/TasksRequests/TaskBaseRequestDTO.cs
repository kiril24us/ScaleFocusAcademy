using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ToDoApplication.DTO.DTO.Requests.TasksRequests
{
    public class TaskBaseRequestDTO
    {
        [Required]
        public int ToDoListId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 10 symbols!")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 100 symbols!")]
        public string Description { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool IsComplete { get; set; }
    }
}
