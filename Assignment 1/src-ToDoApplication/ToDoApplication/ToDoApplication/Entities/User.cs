using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToDoApplication.Common;

namespace ToDoApplication.Entities
{
    public class User : BaseModel
    {
        public User()
        {
            this.ToDoLists = new HashSet<ToDoList>();
            this.SharedToDoLists = new HashSet<ToDoList>();
        }
        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Role Role { get; set; }

        public ICollection<ToDoList> ToDoLists { get; set; }

        public ICollection<ToDoList> SharedToDoLists { get; set; }
    }
}
