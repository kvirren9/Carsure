using System.ComponentModel.DataAnnotations;

namespace Carsure.Models;

public class ContactSellerViewModel
{
    public int AdId { get; set; }
    public string AdTitle { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string SellerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ämne är obligatoriskt")]
    [StringLength(200, ErrorMessage = "Ämnet får inte vara längre än 200 tecken")]
    [Display(Name = "Ämne")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Meddelande är obligatoriskt")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Meddelandet måste vara mellan 10 och 2000 tecken")]
    [Display(Name = "Meddelande")]
    public string Message { get; set; } = string.Empty;

    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
}
