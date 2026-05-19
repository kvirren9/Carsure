using System.ComponentModel.DataAnnotations;

namespace Carsure.Models;

public class EditAdViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Titel är obligatoriskt")]
    [StringLength(200, ErrorMessage = "Titeln får inte vara längre än 200 tecken")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Beskrivning är obligatoriskt")]
    [StringLength(2000, ErrorMessage = "Beskrivningen får inte vara längre än 2000 tecken")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pris är obligatoriskt")]
    [Range(1, 10_000_000, ErrorMessage = "Priset måste vara mellan 1 och 10 000 000")]
    public decimal Price { get; set; }

    public AdStatus Status { get; set; } = AdStatus.PUBLISHED;

    [Required(ErrorMessage = "Registreringsnummer är obligatoriskt")]
    [StringLength(20, ErrorMessage = "Registreringsnumret får inte vara längre än 20 tecken")]
    [Display(Name = "Registreringsnummer")]
    public string RegNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Märke är obligatoriskt")]
    [StringLength(100)]
    [Display(Name = "Märke")]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Modell är obligatoriskt")]
    [StringLength(100)]
    [Display(Name = "Modell")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "År är obligatoriskt")]
    [Range(1900, 2100, ErrorMessage = "Ange ett giltigt år")]
    [Display(Name = "År")]
    public int Year { get; set; } = DateTime.Now.Year;

    [Required(ErrorMessage = "Miltal är obligatoriskt")]
    [StringLength(50)]
    [Display(Name = "Miltal (km)")]
    public string MileAge { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bränsletyp är obligatoriskt")]
    [Display(Name = "Bränsletyp")]
    public string FuelType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Växellåda är obligatoriskt")]
    [Display(Name = "Växellåda")]
    public string Transmission { get; set; } = string.Empty;
}
