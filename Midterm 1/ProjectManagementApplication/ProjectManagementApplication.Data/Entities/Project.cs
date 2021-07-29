using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ProjectManagementApplication.Data.Entities
{
    [DataContract]
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
            Teams = new HashSet<Team>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        [DataMember]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
