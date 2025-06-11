using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SocialMediaManager.Models;

namespace SocialMediaManager.Controllers;

/// <summary>
/// Controller for general site navigation and accessing other controllers views.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Injects the logger.
    /// </summary>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Homepage view.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Privacy policy view. For later...
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Error view.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
