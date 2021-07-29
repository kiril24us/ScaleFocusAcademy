﻿using ProjectManagementApplication.Data.Enums;

namespace ProjectManagementApplication.DTO.Responses.UserResponses
{
    public class UserGetAllResponseDTO
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }
    }
}
