using System.ComponentModel.DataAnnotations;

namespace ToDoApplication.DTO.DTO.Requests.ToDoListsRequests
{
    public class ShareToDoListRequestDTO
    {
        [Required]
        public int UserId { get; set; }
    }
}

