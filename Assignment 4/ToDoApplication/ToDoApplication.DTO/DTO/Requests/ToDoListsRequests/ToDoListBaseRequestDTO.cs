using System.ComponentModel.DataAnnotations;


namespace ToDoApplication.DTO.DTO.Requests.ToDoListsRequests
{
    public abstract class ToDoListBaseRequestDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 30 symbols!")]
        public string Title { get; set; }
    }
}
