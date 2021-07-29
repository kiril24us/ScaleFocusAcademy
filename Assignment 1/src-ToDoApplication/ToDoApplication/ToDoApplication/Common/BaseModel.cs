using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoApplication.Common
{
    public abstract class BaseModel
    {
        public int Id { get; set; }     

        public int CreatorId { get; set; }

        public int IdOfUserLastChange { get; set; }

        public DateTime DateOfLastChange { get; set; }

        public DateTime DateOfCreation { get; set; }
    }
}
