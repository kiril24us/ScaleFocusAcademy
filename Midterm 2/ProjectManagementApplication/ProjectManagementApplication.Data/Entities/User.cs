using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Data.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            Teams = new HashSet<Team>();
            Tasks = new HashSet<Task>();
        }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
