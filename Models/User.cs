using Microsoft.AspNetCore.Identity;
using System;

namespace SocialMediaManager.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Identity User already has email, username, password, etc.
    }
}