using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ToDoApplication.Data.Entities
{
    [DataContract]
    public class Task : BaseModel
    {
        public Task()
        {
            SharedTasks = new HashSet<UserTask>();
        }

        [Key]
        public int TaskId { get; set; }

        [DataMember]
        [Required]
        [ForeignKey("ToDoListId")]
        public int ToDoListId { get; set; }

        public virtual ToDoList ToDoList { get; set; }

        [DataMember]
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 10 symbols!")]
        public string Title { get; set; }

        [DataMember]
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 100 symbols!")]
        public string Description { get; set; }

        [DataMember]
        [Required]
        public bool IsComplete { get; set; }        

        public virtual User User { get; set; }

        public virtual ICollection<UserTask> SharedTasks { get; set; }
    }
}
