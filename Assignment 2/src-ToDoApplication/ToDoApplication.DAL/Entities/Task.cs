using ToDoApplication.DAL.Common;

namespace ToDoApplication.DAL.Entities
{
    public class Task : BaseModel
    {
        public int ToDoListId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }
    }
}
