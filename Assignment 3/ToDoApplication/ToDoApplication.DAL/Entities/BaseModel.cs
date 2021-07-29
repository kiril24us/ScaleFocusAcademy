using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApplication.DAL.Entities
{
    public abstract class BaseModel
    {
        public BaseModel()
        {
            CreatedOn = DateTime.Now;
        }

        [Required]
        public int CreatedById { get; set; }

        [Required]
        public int LastModifiedById { get; set; }

        [Required]
        public DateTime LastModifiedOn { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
