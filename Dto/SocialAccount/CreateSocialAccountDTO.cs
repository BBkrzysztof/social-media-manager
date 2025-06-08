using System.ComponentModel.DataAnnotations;
using SocialMediaManager.Enum;

namespace SocialMediaManager.Dto.SocialAccount;

public class CreateSocialAccountDTO
{
    [Required]
    public string UsernameOnPlatform { get; set; } = string.Empty;

    [Required]
    public string AccessToken { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }

    [Required]
    public Platform Platforms { get; set; }
}