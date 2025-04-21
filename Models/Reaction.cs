namespace SocialMediaManager.Models;

public class Reaction: BaseModel
{
    public required string Type {get; set;}//@todo refactor to enum
    public int Count {get; set;}

    //relation
    public Guid EngagementMetricsId {get; set;}
    public required EngagementMetrics EngagementMetrics {get; set;}
}