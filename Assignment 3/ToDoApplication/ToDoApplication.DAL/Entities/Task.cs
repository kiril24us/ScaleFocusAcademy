using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApplication.DAL.Entities
{
    public class Task : BaseModel
    {
        [Key]
        public int TaskId { get; set; }
    
        [Required]
        [ForeignKey("ToDoListId")]
        public int ToDoListId { get; set; }

        public virtual ToDoList ToDoList { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 10 symbols!")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 100 symbols!")]
        public string Description { get; set; }

        [Required]
        public bool IsComplete { get; set; }        

        public virtual User User { get; set; }

        public virtual ICollection<UserTask> SharedTasks { get; set; }
    }
}
