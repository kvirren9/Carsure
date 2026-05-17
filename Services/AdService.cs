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


    public void CreateAdWithCar(Ad ad, Car car)
    {
        ad.CreatedAt = DateTime.UtcNow;
        ad.Car = car;
        _dbContext.Cars.Add(car);
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

    public IReadOnlyList<Ad> SearchAds(AdSearchViewModel filters)
    {
        var query = _dbContext.Ads
            .Where(a => a.Status == AdStatus.PUBLISHED)
            .Include(a => a.Car)
            .Include(a => a.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Keyword))
        {
            var kw = filters.Keyword.Trim().ToLower();
            query = query.Where(a =>
                a.Title.ToLower().Contains(kw) ||
                a.Description.ToLower().Contains(kw) ||
                a.Car.Brand.ToLower().Contains(kw) ||
                a.Car.Model.ToLower().Contains(kw));
        }

        if (!string.IsNullOrWhiteSpace(filters.Brand))
            query = query.Where(a => a.Car.Brand.ToLower() == filters.Brand.ToLower());

        if (!string.IsNullOrWhiteSpace(filters.Model))
            query = query.Where(a => a.Car.Model.ToLower() == filters.Model.ToLower());

        if (filters.MinYear.HasValue)
            query = query.Where(a => a.Car.Year >= filters.MinYear.Value);

        if (filters.MaxYear.HasValue)
            query = query.Where(a => a.Car.Year <= filters.MaxYear.Value);

        if (filters.MinPrice.HasValue)
            query = query.Where(a => a.Price >= filters.MinPrice.Value);

        if (filters.MaxPrice.HasValue)
            query = query.Where(a => a.Price <= filters.MaxPrice.Value);

        return query.OrderByDescending(a => a.CreatedAt).ToList();
    }

    public List<string> GetPublishedBrands() => _dbContext.Ads
        .Where(a => a.Status == AdStatus.PUBLISHED)
        .Include(a => a.Car)
        .Select(a => a.Car.Brand)
        .Distinct()
        .OrderBy(b => b)
        .ToList();

    public List<string> GetPublishedModels(string? brand = null)
    {
        var query = _dbContext.Ads
            .Where(a => a.Status == AdStatus.PUBLISHED)
            .Include(a => a.Car)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(a => a.Car.Brand.ToLower() == brand.ToLower());

        return query.Select(a => a.Car.Model)
            .Distinct()
            .OrderBy(m => m)
            .ToList();
    }
}
