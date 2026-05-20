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

    // New filters
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }

    // Sorting: "price_asc", "price_desc", "date_desc" (default), "date_asc", "mileage_asc", "mileage_desc"
    public string? SortBy { get; set; }

    // Pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
