namespace SocialMediaManager.Models;

public class ViewSnapshot: BaseModel
{
    public required int ViewCount {get; set;}
    public required DateTime SnapshotTime {get; set;}

    //relations 
    public Guid SocialPostId {get; set;}
    public required SocialPost SocialPost {get; set;}
}