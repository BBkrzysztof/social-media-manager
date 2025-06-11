using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SocialMediaManager.Models;
using SocialMediaManager.Dto.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Data;
using Microsoft.AspNetCore.Authorization;

namespace SocialMediaManager.Controllers;

/// <summary>
/// Handles user authentication (register, login, logout).
/// </summary>
[Route("auth")]
public class AuthController : Controller
{
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

    /// <summary>
    /// Constructor with ...
    /// </summary>
    public AuthController(
        UserManager<User> userManager, 
        SignInManager<User> signInManager,
        AppDbContext context
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }


    /// <summary>
    /// Displays the registration form.
    /// </summary>
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

        /// <summary>
        /// Handles user registration of an user.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Displays the login form.
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles login request using credentials.
        /// </summary>
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        /// <summary>
        /// Handles logout from app. (uses get)
        /// </summary>
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
}
