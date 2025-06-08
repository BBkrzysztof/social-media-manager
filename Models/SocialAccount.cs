
using SocialMediaManager.Enum;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaManager.Models;

public class SocialAccount : BaseModel
{
    public string UsernameOnPlatform { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }

    public Platform Platforms { get; set; }

    // Foreign keys

    public string UserId { get; set; } = string.Empty;

    public required User User { get; set; }

    // Back relations
    public ICollection<SocialPost> SocialPosts { get; set; } = new List<SocialPost>();
}
