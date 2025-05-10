namespace SocialMediaManager.Models;

public class SocialPlatform : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public string? Logo { get; set; }

    // Back relations
    public ICollection<SocialAccount> SocialAccounts { get; set; } = new List<SocialAccount>();
}
