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
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
