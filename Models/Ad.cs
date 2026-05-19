namespace Carsure.Models;

public class Ad
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public AdStatus Status { get; set; } = AdStatus.DRAFT;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public int? UserId { get; set; }
    public User? User { get; set; }

    public List<string> GetImageUrls(int maxCount = 10)
    {
        if (string.IsNullOrWhiteSpace(ImageUrl) || maxCount <= 0)
            return new List<string>();

        var parts = ImageUrl
            .Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(maxCount)
            .ToList();

        return parts;
    }
}
