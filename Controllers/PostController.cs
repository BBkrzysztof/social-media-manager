using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SocialMediaManager.Models;
using SocialMediaManager.Dto.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Data;
using Microsoft.AspNetCore.Authorization;
using SocialMediaManager.Dto.Post;

namespace SocialMediaManager.Controllers;

/// <summary>
/// Controller responsible for managing posts: listing, creating, updating and deleting.
/// </summary>
[Route("post")]
[Authorize]
public class PostController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly UserManager<User> _userManager;


    /// <summary>
    /// Constructor with dependencies injected.
    /// </summary>
    public PostController(
        UserManager<User> userManager,
        AppDbContext context,
        IWebHostEnvironment env
    )
    {
        _context = context;
        _env = env;
        _userManager = userManager;
    }


    /// <summary>
    /// Returns paginated list of posts belonging to the current user as JSON. Can then be accessed with JavaScript fetch() without page reloading.
    /// </summary>
    [HttpGet("/list")]
    public IActionResult Get(int page = 1, int pageSize = 10)
    {
        string userId = _userManager.GetUserId(User);

        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var totalItems = _context.Posts.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var posts = _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Where(p => p.UserId == userId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList()
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Content,
                p.ScheduledAt,
                p.CreatedAt,
                p.MediaUrls,
                Platforms = p.Platforms.Select(pl => System.Enum.GetName(typeof(SocialMediaManager.Enum.Platform), pl)).ToList()
            })
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

    /// <summary>
    /// Displays the list view of all posts.
    /// </summary>
    [HttpGet("")]
    public IActionResult List()
    {
        return View("List");
    }


    /// <summary>
    /// Displays the form to create a new post.
    /// </summary>
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles form submission to create a new post.
    /// </summary>
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var mediaUrls = "";

        if (dto.MediaUrls is not null)
        {
            var file = dto.MediaUrls;

            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            mediaUrls = "/uploads/" + fileName;

        }

        string userId = _userManager.GetUserId(User);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            ScheduledAt = dto.ScheduledAt,
            MediaUrls = mediaUrls,
            Platforms = dto.Platforms,
            UserId = userId,
            User = user
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("Update", new { id = post.Id });
    }

    /// <summary>
    /// Displays the form to update an existing post.
    /// </summary>
    [HttpGet("update/{id}")]
    public IActionResult Update(Guid id)
    {
        var model = _context.Posts.FirstOrDefault(e => e.Id == id);
        if (model == null)
            return NotFound();

        var dto = new CreatePostDTO
        {
            Title = model.Title,
            Content = model.Content,
            ScheduledAt = model.ScheduledAt,
            Platforms = model.Platforms
            // MediaUrls skip - backend cannot fill form file field
        };

        ViewData["PostId"] = model.Id;
        return View(dto);
    }

    /// <summary>
    /// Handles form submission to update an existing post.
    /// </summary>
    [HttpPost("update/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, CreatePostDTO dto)
    {
        var existingPost = await _context.Posts.FindAsync(id);
        if (existingPost == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        existingPost.Title = dto.Title;
        existingPost.Content = dto.Content;
        existingPost.ScheduledAt = dto.ScheduledAt;
        existingPost.Platforms = dto.Platforms;

        if (dto.MediaUrls is not null)
        {
            var file = dto.MediaUrls;

            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path)!);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            existingPost.MediaUrls = "/uploads/" + fileName;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("List");
    }

    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("List");
    }
}
