using Carsure.Data;
using Carsure.Models;

namespace Carsure.Services;

public class AdService
{
    private readonly ApplicationDbContext _dbContext;

    public AdService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Ad> GetAds() => _dbContext.Ads
        .OrderByDescending(ad => ad.CreatedAt)
        .ToList();

    public Ad? GetAdById(int id) => _dbContext.Ads.FirstOrDefault(ad => ad.Id == id);

    public void CreateAd(Ad ad)
    {
        ad.CreatedAt = DateTime.UtcNow;
        _dbContext.Ads.Add(ad);
        _dbContext.SaveChanges();
    }

    public void UpdateAd(Ad ad)
    {
        var existingAd = _dbContext.Ads.FirstOrDefault(a => a.Id == ad.Id);
        if (existingAd is null)
            return;

        existingAd.Title = ad.Title;
        existingAd.Description = ad.Description;
        existingAd.Price = ad.Price;

        _dbContext.SaveChanges();
    }

    public void DeleteAd(int id)
    {
        var ad = _dbContext.Ads.FirstOrDefault(a => a.Id == id);
        if (ad is null)
            return;

        _dbContext.Ads.Remove(ad);
        _dbContext.SaveChanges();
    }
}
