namespace SocialMediaManager.Models;

using Microsoft.AspNetCore.Identity;
using System;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Back relations
    public ICollection<SocialAccount> SocialAccounts { get; set; } = new List<SocialAccount>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();

    // Identity User already has email, username, password, etc.
}
