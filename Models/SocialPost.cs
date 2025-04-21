namespace SocialMediaManager.Models;

public class SocialPost: BaseModel
{
    public required string Status {get; set;} //@todo refactor to enum 

    //relations 
    public Guid SocialAccountId {get; set;}
    public required SocialAccount SocialAccount { get; set;}

    public Guid PostId {get; set;}
    public required Post Post {get; set;}

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<ViewSnapshot> ViewSnapshots { get; set; } = new List<ViewSnapshot>();

    public ICollection<EngagementMetrics> EngagementMetricses { get; set; } = new List<EngagementMetrics>();
}
