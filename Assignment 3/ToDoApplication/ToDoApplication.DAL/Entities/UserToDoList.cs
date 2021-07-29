namespace ToDoApplication.DAL.Entities
{
    public class UserToDoList
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int ToDoListId { get; set; }

        public virtual ToDoList ToDoList { get; set; }
    }
}
