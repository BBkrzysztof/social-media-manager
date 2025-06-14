namespace SocialMediaManager.Services;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SocialMediaManager.Models;
using SocialMediaManager.Services.Exceptions;


public class TwitterService : ISocialMediaService
{
    private readonly HttpClient _httpClient;

    public TwitterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> PostAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/2/tweets");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", account.AccessToken);
            request.Content = new StringContent(
                JsonSerializer.Serialize(new { text = post.Post.Content }),
                Encoding.UTF8,
                "application/json"
            ); //TODO make it work with twitter

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            //TODO Implement API post
            return "";
        }
        catch (Exception ex)
        {
            throw new SocialMediaException("ERROR in post on Twitter ", ex);
        }
    }

    public async Task<IList<Reaction>> GetReactionsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        return new List<Reaction>(); //TODO Implement API get
    }

    public async Task<IList<Comment>> GetCommentsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        return new List<Comment>(); //TODO Implement API get
    }

    public async Task<int> GetViewsAsync(SocialAccount account, SocialPost post, CancellationToken cancellationToken = default)
    {
        return 0; //TODO Implement API get
    }
}