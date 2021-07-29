using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ProjectManagementApplication.Data.Entities
{
    [DataContract]
    public class Team
    {
        public Team()
        {
            Members = new HashSet<User>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Team name must be between 1 and 20 symbols!")]
        public string Name { get; set; }
       
        public bool IsActive { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<User> Members { get; set; }
    }
}
