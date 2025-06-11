using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Data;
using SocialMediaManager.Models;
using SocialMediaManager.Enum;
using System.ComponentModel;
using System.Reflection;

namespace SocialMediaManager.Controllers;

/// <summary>
/// Controls linking and unlinking of social media accounts to the logged-in user.
/// Displays connected accounts and provides OAuth redirects.
/// </summary>
[Authorize]
[Route("social")]
public class SocialLinkController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Constructs the controller with database context and identity manager.
    /// </summary>
    public SocialLinkController(AppDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Displays a view with the user's currently linked social media accounts.
    /// </summary>
    [HttpGet("OAuthPlatforms")]
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        var accounts = _context.SocialAccounts
            .Where(a => a.UserId == userId)
            .ToList();

        return View("OAuthPlatforms", accounts);
    }

    /// <summary>
    /// Unlinks a given social media account based on platform name.
    /// </summary>
    /// <param name="platform">Platform name in lowercase (e.g., facebook, x)</param>
    [HttpGet("unlink/{platform}")]
    public async Task<IActionResult> Unlink(string platform)
    {
        var userId = _userManager.GetUserId(User);

        if (!System.Enum.TryParse<Platform>(platform, true, out var parsedPlatform))
        {
            return BadRequest("Unknown platform.");
        }

        var account = await _context.SocialAccounts
            .FirstOrDefaultAsync(a => a.UserId == userId && a.Platform == parsedPlatform);

        if (account != null)
        {
            _context.SocialAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
