namespace SocialMediaManager.Models;

public class Comment: BaseModel
{
    public required string UserName {get; set;}

    public required string Content {get; set;}

    //relations 
    public Guid SocialPostId {get; set;}    
    public required SocialPost SocialPost {get; set;} 
}