namespace SocialMediaManager.Services;
using SocialMediaManager.Models;

public interface ISocialMediaService
{
    Task PostAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default);
    Task<IList<Reaction>> GetReactionsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default);
    Task<IList<Comment>> GetCommentsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default);
    Task<int> GetViewsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default);
}
