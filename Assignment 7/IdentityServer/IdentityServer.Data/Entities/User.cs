using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer.Data.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; set; }
    }
}