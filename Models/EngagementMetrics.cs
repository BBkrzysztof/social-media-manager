namespace SocialMediaManager.Models;

public class EngagementMetrics: BaseModel
{
    public int Likes {get; set;} = 0;

    public int Shares {get; set;} = 0;

    public int Comments {get; set;} = 0;

    public int LastChecked {get; set;} = 0;
    

    //relations 
    public Guid SocialPostId {get; set;}
    public required SocialPost SocialPost {get; set;}

    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

}