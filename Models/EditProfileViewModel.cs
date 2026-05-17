using System.ComponentModel.DataAnnotations;

namespace Carsure.Models;

public class EditProfileViewModel
{
    [Required(ErrorMessage = "Namn är obligatoriskt")]
    [StringLength(100, ErrorMessage = "Namnet får inte vara längre än 100 tecken")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-post är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken")]
    public string? NewPassword { get; set; }

    [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte")]
    public string? ConfirmPassword { get; set; }
}
