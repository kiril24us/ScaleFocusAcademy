using System;

namespace ToDoApplication.DTO.DTO.Responses.TaskResponses
{
    public class TaskGetAllResponseDTO
    {
        public int TaskId { get; set; }

        public int ToDoListId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }

        public int CreatedById { get; set; }

        public int LastModifiedById { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
