using ProjectManagementApplication.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Data.Entities
{
    public class User
    {
        public User()
        {
            Teams = new HashSet<Team>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }

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

        [Required]
        public Role Role { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
