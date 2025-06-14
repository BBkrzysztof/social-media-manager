using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SocialMediaManager.Models;
using SocialMediaManager.Services.Exceptions;
using Newtonsoft.Json;

namespace SocialMediaManager.Services;

/// <summary>
/// FacebookService provides methods to interact with the Facebook Graph API
/// including posting to a user's page and retrieving engagement.
/// </summary>
public class FacebookService : ISocialMediaService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<FacebookService> _logger;

    /// <summary>
    /// Constructs FacebookService with HttpClient and logger.
    /// </summary>
    public FacebookService(HttpClient httpClient, ILogger<FacebookService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Publishes a post to the Facebook Page associated with the given SocialAccount.
    /// </summary>
    /// <param name="account">Social account linked to a Facebook Page.</param>
    /// <param name="post">Post content to be published.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    public async Task<string> PostAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://graph.facebook.com/v23.0/{account.UsernameOnPlatform}/feed");

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "message", post.Post.Content },
                { "access_token", account.AccessToken }
            });

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FacebookPostResponse>(body);

            return result.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Facebook post failed");
            throw new SocialMediaException("Failed to post on Facebook.", ex);
        }

    }

    /// <summary>
    /// Gets reactions for a Facebook post (Not implemented).
    /// </summary>
    public async Task<IList<Reaction>> GetReactionsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        return new List<Reaction>();
    }

    /// <summary>
    /// Gets comments for a Facebook post (Not implemented).
    /// </summary>
    public async Task<IList<Comment>> GetCommentsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        return new List<Comment>();
    }

    /// <summary>
    /// Gets number of views for a Facebook post (Not implemented).
    /// </summary>
    public async Task<int> GetViewsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        return 0;
    }

    public class FacebookPostResponse
    {
        public string Id { get; set; }
    }
}
