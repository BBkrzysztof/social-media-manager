using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Data;
using SocialMediaManager.Models;
using System.Text.Json;
using SocialMediaManager.Enum;

namespace SocialMediaManager.Controllers;

/// <summary>
/// Handles Facebook OAuth login and callback for connecting Facebook page to this app.
/// </summary>
[Route("facebook")]
[Authorize]
public class FacebookController : Controller
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public FacebookController(IConfiguration config, AppDbContext context, UserManager<User> userManager)
    {
        _config = config;
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Starts Facebook OAuth flow.
    /// </summary>
    [HttpGet("login")]
    public IActionResult Login()
    {
        var clientId = Environment.GetEnvironmentVariable("PAGES_APP_ID")!;
        var redirectUri = _config["Facebook:RedirectUri"];
        var scope = "pages_manage_posts,pages_read_engagement,pages_show_list";

        var loginUrl =
            $"https://www.facebook.com/v23.0/dialog/oauth?client_id={clientId}&redirect_uri={redirectUri}&scope={scope}&response_type=code";

        return Redirect(loginUrl);
    }

    /// <summary>
    /// Handles OAuth callback and stores Facebook Page access tokens.
    /// </summary>
    [HttpGet("callback")]
    public async Task<IActionResult> Callback(string code)
    {
        var clientId = Environment.GetEnvironmentVariable("PAGES_APP_ID")!;
        var clientSecret = Environment.GetEnvironmentVariable("PAGES_APP_SECRET")!;
        var redirectUri = _config["Facebook:RedirectUri"];

        using var http = new HttpClient();

        var tokenResponse = await http.GetFromJsonAsync<FacebookTokenResponse>(
            $"https://graph.facebook.com/v23.0/oauth/access_token?client_id={clientId}&redirect_uri={redirectUri}&client_secret={clientSecret}&code={code}");

        var userResponse = await http.GetFromJsonAsync<FacebookAccountList>(
            $"https://graph.facebook.com/v23.0/me/accounts?access_token={tokenResponse.access_token}");

        var userId = _userManager.GetUserId(User);
        var user = await _context.Users.FindAsync(userId);

        foreach (var page in userResponse.data)
        {
            _context.SocialAccounts.Add(new SocialAccount
            {
                UsernameOnPlatform = page.id!,
                AccessToken = page.access_token!,
                Platform = Platform.Facebook,
                UserId = userId,
                User = user!
            });
        }

        await _context.SaveChangesAsync();

        return Redirect("/post");
    }

    /// <summary>
    /// Response model for access token exchange.
    /// </summary>
    public class FacebookTokenResponse
    {
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public int? expires_in { get; set; }
    }

    /// <summary>
    /// Wrapper for list of pages.
    /// </summary>
    public class FacebookAccountList
    {
        public List<FacebookPage> data { get; set; } = new();
    }

    /// <summary>
    /// Facebook page model with access token.
    /// </summary>
    public class FacebookPage
    {
        public string? id { get; set; } = string.Empty;
        public string? name { get; set; } = string.Empty;
        public string? access_token { get; set; } = string.Empty;
    }
}
