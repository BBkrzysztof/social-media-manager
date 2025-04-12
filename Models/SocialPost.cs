namespace SocialMediaManager.Models;

class SocialPost: BaseModel
{
    public required string Status {get; set;} //@todo refactor to enum 

    //relations 
    //@todo uncoment when models SocialAccount, Post exists
    public Guid SocialAccountId {get; set;}
    // public SocialAccount {get; set;}

    public Guid PostId {get; set;}
    // public Post Post {get; set;}

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<ViewSnapshot> ViewSnapshots { get; set; } = new List<ViewSnapshot>();

    public ICollection<EngagementMetrics> EngagementMetricses { get; set; } = new List<EngagementMetrics>();
}