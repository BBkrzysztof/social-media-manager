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

[Route("post")]
[Authorize]
public class PostController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly UserManager<User> _userManager;


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

    [HttpGet("update/{id}")]
    public IActionResult Update(Guid id)
    {
        var model = _context.Posts.FirstOrDefault(e => e.Id == id);
        if (model == null)
        {
            // return NotFound();
        }

        return View(model);
    }


}
