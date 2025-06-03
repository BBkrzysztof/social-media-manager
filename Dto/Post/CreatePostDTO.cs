using System.ComponentModel.DataAnnotations;
using SocialMediaManager.Enum;
 
namespace SocialMediaManager.Dto.Post;

public class CreatePostDTO
{
    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    public DateTime? ScheduledAt { get; set; }

    public string? MediaUrls { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one platform must be selected.")]
    public List<Platform> Platforms { get; set; } = new();
}