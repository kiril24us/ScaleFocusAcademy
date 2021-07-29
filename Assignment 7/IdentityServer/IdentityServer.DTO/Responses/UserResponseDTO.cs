using System;

namespace IdentityServer.DTO.Responses
{
    public class UserResponseDTO 
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }
    }
}
