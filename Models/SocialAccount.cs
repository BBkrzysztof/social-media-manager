using SocialMediaManager.Enum;

namespace SocialMediaManager.Models;

/// <summary>
/// Represents a social media account linked to a user, including access token information
/// and the platform it belongs to.
/// </summary>
public class SocialAccount : BaseModel
{
    /// <summary>
    /// The platform type (e.g., Facebook, X, Instagram, Threads, etc.).
    /// </summary>
    public Platform Platform { get; set; }

    /// <summary>
    /// The username or page ID used on the social media platform.
    /// </summary>
    public string UsernameOnPlatform { get; set; } = string.Empty;

    /// <summary>
    /// The access token for API interactions.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Optional refresh token if given by platform...
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Foreign key to the owning user.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// The owning user of this social account.
    /// </summary>
    public required User User { get; set; }

    /// <summary>
    /// The list of posts made with this account.
    /// </summary>
    public ICollection<SocialPost> SocialPosts { get; set; } = new List<SocialPost>();
}
