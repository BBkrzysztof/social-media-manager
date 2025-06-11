namespace SocialMediaManager.Models;

public class SocialAccount : BaseModel
{
    public string UsernameOnPlatform { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }

    // Foreign keys
    public int PlatformId { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Relations
    public required SocialPlatform SocialPlatform { get; set; }
    public required User User { get; set; }

    // Back relations
    public ICollection<SocialPost> SocialPosts { get; set; } = new List<SocialPost>();
}
