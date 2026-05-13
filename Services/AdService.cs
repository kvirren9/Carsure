using Carsure.Data;
using Carsure.Models;
using Microsoft.EntityFrameworkCore;

namespace Carsure.Services;

public class AdService
{
    private readonly ApplicationDbContext _dbContext;

    public AdService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Ad> GetAds() => _dbContext.Ads
        .Where(a => a.Status == AdStatus.PUBLISHED)
        .Include(a => a.Car)
        .Include(a => a.User)
        .OrderByDescending(ad => ad.CreatedAt)
        .ToList();

    public IReadOnlyList<Ad> GetAllAds() => _dbContext.Ads
        .Include(a => a.Car)
        .Include(a => a.User)
        .OrderByDescending(ad => ad.CreatedAt)
        .ToList();

    public IReadOnlyList<Ad> GetAdsByUser(int userId) => _dbContext.Ads
        .Where(a => a.UserId == userId)
        .Include(a => a.Car)
        .OrderByDescending(ad => ad.CreatedAt)
        .ToList();

    public Ad? GetAdById(int id) => _dbContext.Ads
        .Where(a => a.Status == AdStatus.PUBLISHED)
        .Include(a => a.Car)
        .Include(a => a.User)
        .FirstOrDefault(ad => ad.Id == id);

    public Ad? GetAdByIdAnyStatus(int id) => _dbContext.Ads
        .Include(a => a.Car)
        .Include(a => a.User)
        .FirstOrDefault(ad => ad.Id == id);

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


    public bool CanModifyAd(int adId, int userId, bool isAdmin)
    {
        if (isAdmin) return true;
        var ad = _dbContext.Ads.FirstOrDefault(a => a.Id == adId);
        return ad?.UserId == userId;
    }
}
