using ProjectManagementApplication.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Data.Entities
{
    public class Task
    {
        public Task()
        {
            WorkLogs = new HashSet<WorkLog>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string CreatorId { get; set; }

        public string AssigneeId { get; set; }

        public virtual User Assignee { get; set; }

        [Required]
        public Status Status { get; set; }

        public virtual ICollection<WorkLog> WorkLogs { get; set; }
    }
}