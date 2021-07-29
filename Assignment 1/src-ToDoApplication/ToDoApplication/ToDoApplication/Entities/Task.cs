using System;
using System.Collections.Generic;
using System.Text;
using ToDoApplication.Common;

namespace ToDoApplication.Entities
{
    public class Task : BaseModel
    {
        public int ToDoListId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }
    }
}
