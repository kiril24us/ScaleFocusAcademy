using System;

namespace ProjectManagementApplication.DTO.Responses.WorkLogResponses
{
    public class WorkLogCreateResponseDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public string StartDate { get; set; }       

        public int TaskId { get; set; }

        public int UserId { get; set; }
    }
}
