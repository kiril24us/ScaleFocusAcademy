using System.Collections.Generic;
using ToDoApplication.DAL.Common;

namespace ToDoApplication.DAL.Entities
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
