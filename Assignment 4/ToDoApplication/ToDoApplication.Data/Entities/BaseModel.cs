using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ToDoApplication.Data.Entities
{
    [DataContract]
    public abstract class BaseModel
    {
        public BaseModel()
        {
            CreatedOn = DateTime.Now;
        }

        [DataMember]
        [Required]
        public int CreatedById { get; set; }

        [DataMember]
        [Required]
        public int LastModifiedById { get; set; }

        [DataMember]
        [Required]
        public DateTime LastModifiedOn { get; set; }

        [DataMember]
        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
