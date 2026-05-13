namespace Carsure.Models;

public class Ad
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public AdStatus Status { get; set; } = AdStatus.DRAFT;

    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    public int? UserId { get; set; }
    public User? User { get; set; }
}
