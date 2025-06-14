using System.ComponentModel.DataAnnotations;

namespace SocialMediaManager.Dto.Auth;

public class RegisterDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords mismatch.")]
    public string ConfirmPassword { get; set; }
}