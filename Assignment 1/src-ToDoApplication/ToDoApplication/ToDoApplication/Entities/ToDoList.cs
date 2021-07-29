using System.Collections.Generic;
using ToDoApplication.Common;

namespace ToDoApplication.Entities
{
    public class ToDoList : BaseModel
    {
        public ToDoList()
        {
            Tasks = new HashSet<Task>();
        }

        public string Title { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
