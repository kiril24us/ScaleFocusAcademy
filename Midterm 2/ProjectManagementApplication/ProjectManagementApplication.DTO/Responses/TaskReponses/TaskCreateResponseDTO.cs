namespace ProjectManagementApplication.DTO.Responses.TaskReponses
{
    public class TaskCreateResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ProjectId { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }
    }
}
