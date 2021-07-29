using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Data.Entities
{
    public class WorkLog
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TimeSpan TimeSpent { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public bool IsActive { get; set; }

        public int TaskId { get; set; }

        public virtual Task Task { get; set; }

        public int UserId { get; set; }
    } 
}
