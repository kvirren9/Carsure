using System.ComponentModel.DataAnnotations;

namespace Carsure.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Namn är obligatoriskt")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-post är obligatoriskt")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;
}