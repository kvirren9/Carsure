namespace Carsure.Models;

public class UserProfileViewModel
{
    public User User { get; set; } = null!;
    public IReadOnlyList<Ad> Ads { get; set; } = [];

    public int TotalAds => Ads.Count;
    public int PublishedAds => Ads.Count(a => a.Status == AdStatus.PUBLISHED);
    public int DraftAds => Ads.Count(a => a.Status == AdStatus.DRAFT);
}
