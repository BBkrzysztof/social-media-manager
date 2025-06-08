using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SocialMediaManager.Data;
using SocialMediaManager.Dto.Auth;
using SocialMediaManager.Dto.Post;
using SocialMediaManager.Dto.SocialAccount;
using SocialMediaManager.Enum;
using SocialMediaManager.Models;

namespace SocialMediaManager.Controllers;

[Route("social-account")]
[Authorize]
public class SocialAccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly UserManager<User> _userManager;

    public SocialAccountController(
        UserManager<User> userManager,
        AppDbContext context,
        IWebHostEnvironment env
    )
    {
        _context = context;
        _env = env;
        _userManager = userManager;
    }

    [HttpGet("/list")]
    public IActionResult Get(int page = 1, int pageSize = 10)
    {
        string userId = _userManager.GetUserId(User);

        if (page <= 0)
            page = 1;
        if (pageSize <= 0)
            pageSize = 10;

        var totalItems = _context.Posts.Count();
        var totalPages = (int) Math.Ceiling(totalItems / (double) pageSize);

        var posts = _context.SocialAccounts
            .OrderByDescending(p => p.CreatedAt)
            .Where(p => p.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = new
        {
            currentPage = page,
            pageSize = pageSize,
            totalItems = totalItems,
            totalPages = totalPages,
            data = posts
        };

        return Ok(response);
    }

    [HttpGet("")]
    public IActionResult List()
    {
        return View();
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSocialAccountDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        string userId = _userManager.GetUserId(User);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var entity = new SocialAccount
        {
            UsernameOnPlatform = dto.UsernameOnPlatform,
            AccessToken = dto.AccessToken,
            RefreshToken = dto.RefreshToken,
            Platforms = dto.Platforms,
            UserId = userId,
            User = user,
        };

        _context.SocialAccounts.Add(entity);
        _context.SaveChanges();

        return RedirectToAction("Update", new { id = entity.Id });
    }

    [HttpGet("update/{id}")]
    public IActionResult Update(Guid id)
    {
        string userId = _userManager.GetUserId(User);
        var model = _context.SocialAccounts.FirstOrDefault(e => e.Id == id && e.UserId == userId);

        if (model == null)
        {
            // return NotFound();
        }

        return View(new UpdateSocialAccountDTO
        {
            Id = model.Id,
            UsernameOnPlatform = model.UsernameOnPlatform,
            AccessToken = model.AccessToken,
            RefreshToken = model.RefreshToken,
            Platforms = model.Platforms,
        });
    }

    [HttpPost("update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAction(UpdateSocialAccountDTO dto)
    {
        string userId = _userManager.GetUserId(User);
        var entity = _context.SocialAccounts.FirstOrDefault(a => a.Id == dto.Id && a.UserId == userId);

        if (entity == null)
            return NotFound();


        if (!ModelState.IsValid)
            return View(dto);

        entity.UsernameOnPlatform = dto.UsernameOnPlatform;
        entity.AccessToken = dto.AccessToken;
        entity.RefreshToken = dto.RefreshToken;
        entity.Platforms = dto.Platforms;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();

        return RedirectToAction("Update", new { id = entity.Id });
    }
}
