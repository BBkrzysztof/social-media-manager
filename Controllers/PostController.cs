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

    [HttpGet("list")]
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

            mediaUrls.Add("/uploads/" + fileName);

        }

        string userId = _userManager.GetUserId(User);

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            ScheduledAt = dto.ScheduledAt,
            MediaUrls = mediaUrls,
            Platforms = dto.Platforms,
            UserId = userId,
        };



        return View();
    }
    
    [HttpGet("update")]
    public IActionResult Update()
    {
        return View();
    }
}
