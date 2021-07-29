using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ToDoApplication.Data.Entities
{
    [DataContract]
    public class ToDoList : BaseModel
    {
        public ToDoList()
        {
            Tasks = new HashSet<Task>();
            ToDoListsUsers = new HashSet<UserToDoList>();
        }

        [DataMember]
        [Key]
        public int ToDoListId { get; set; }

        [DataMember]
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 30 symbols!")]
        public string Title { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<UserToDoList> ToDoListsUsers { get; set; }

        [DataMember]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
