namespace SocialMediaManager.Models;

using SocialMediaManager.Enum;
using System.ComponentModel.DataAnnotations;


public class Post : BaseModel
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime? ScheduledAt { get; set; }

    public string? MediaUrls { get; set; }

    public List<Platform> Platforms { get; set; } = new();

    // Foreign keys
    public string UserId { get; set; } = string.Empty;

    // Relations
    public required User User { get; set; }

    // Back relations
    public ICollection<SocialPost> SocialPosts { get; set; } = new List<SocialPost>();
}
