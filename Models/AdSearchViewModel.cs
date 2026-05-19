namespace Carsure.Models;

public class AdSearchViewModel
{
    public string? Keyword { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinMileage { get; set; }
    public int? MaxMileage { get; set; }
}
