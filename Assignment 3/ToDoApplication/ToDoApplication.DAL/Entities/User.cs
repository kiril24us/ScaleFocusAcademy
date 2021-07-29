using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoApplication.DAL.Enum;

namespace ToDoApplication.DAL.Entities
{
    public class User : BaseModel
    {
        public User()
        {
            UserSharedLists = new HashSet<UserToDoList>();
            UserSharedTasks = new HashSet<UserTask>();
            ToDoLists = new HashSet<ToDoList>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Username must be between 1 and 10 symbols!")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 20 symbols!")]
        public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        public Role Role { get; set; }

        public virtual ICollection<UserToDoList> UserSharedLists { get; set; }

        public virtual ICollection<ToDoList> ToDoLists { get; set; }

        public virtual ICollection<UserTask> UserSharedTasks { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
