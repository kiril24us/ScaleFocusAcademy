using System;
using ToDoApplication.Data.Enum;

namespace ToDoApplication.DTO.DTO.Responses.UserResponses
{
    public class UserGetAllResponseDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Role Role { get; set; }

        public int CreatedById { get; set; }

        public int LastModifiedById { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
