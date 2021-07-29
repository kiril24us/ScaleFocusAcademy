using System;
using System.Collections.Generic;
using ToDoApplication.DTO.DTO.Responses.TaskResponses;

namespace ToDoApplication.DTO.DTO.Responses.ToDoListResponses
{
    public class ToDoListGetAllResponseDTO
    {
        public ToDoListGetAllResponseDTO()
        {
            Tasks = new HashSet<TaskGetAllForToDoListResponseDTO>();
        }

        public int ToDoListId { get; set; }
        
        public string Title { get; set; }

        public int CreatedById { get; set; }

        public int LastModifiedById { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<TaskGetAllForToDoListResponseDTO> Tasks { get; set; }   
    }
}
