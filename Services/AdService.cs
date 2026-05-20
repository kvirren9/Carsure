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
        var existingAd = _dbContext.Ads
            .Include(a => a.Car)
            .FirstOrDefault(a => a.Id == ad.Id);
        if (existingAd is null)
            return;

        existingAd.Title = ad.Title;
        existingAd.Description = ad.Description;
        existingAd.Price = ad.Price;
        existingAd.Status = ad.Status;
        existingAd.City = ad.City;
        existingAd.Region = ad.Region;

        if (existingAd.Car is not null && ad.Car is not null)
        {
            existingAd.Car.RegNumber = ad.Car.RegNumber;
            existingAd.Car.Brand = ad.Car.Brand;
            existingAd.Car.Model = ad.Car.Model;
            existingAd.Car.Year = ad.Car.Year;
            existingAd.Car.MileAge = ad.Car.MileAge;
            existingAd.Car.FuelType = ad.Car.FuelType;
            existingAd.Car.Transmission = ad.Car.Transmission;
            existingAd.Car.Price = ad.Price;
            existingAd.Car.Description = ad.Description;
        }

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

    public PagedResult<Ad> SearchAds(AdSearchViewModel filters)
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

        if (!string.IsNullOrWhiteSpace(filters.FuelType))
            query = query.Where(a => a.Car.FuelType.ToLower() == filters.FuelType.ToLower());

        if (!string.IsNullOrWhiteSpace(filters.Transmission))
            query = query.Where(a => a.Car.Transmission.ToLower() == filters.Transmission.ToLower());

        var results = query.ToList();

        if (filters.MinMileage.HasValue)
            results = results.Where(a => int.TryParse(a.Car.MileAge, out var m) && m >= filters.MinMileage.Value).ToList();

        if (filters.MaxMileage.HasValue)
            results = results.Where(a => int.TryParse(a.Car.MileAge, out var m) && m <= filters.MaxMileage.Value).ToList();

        if (!string.IsNullOrWhiteSpace(filters.City))
            results = results.Where(a => a.City != null && a.City.ToLower() == filters.City.ToLower()).ToList();

        if (!string.IsNullOrWhiteSpace(filters.Region))
            results = results.Where(a => a.Region != null && a.Region.ToLower() == filters.Region.ToLower()).ToList();

        // Sorting
        results = filters.SortBy switch
        {
            "price_asc" => results.OrderBy(a => a.Price).ToList(),
            "price_desc" => results.OrderByDescending(a => a.Price).ToList(),
            "date_asc" => results.OrderBy(a => a.CreatedAt).ToList(),
            "mileage_asc" => results.OrderBy(a => int.TryParse(a.Car.MileAge, out var m) ? m : int.MaxValue).ToList(),
            "mileage_desc" => results.OrderByDescending(a => int.TryParse(a.Car.MileAge, out var m) ? m : -1).ToList(),
            _ => results.OrderByDescending(a => a.CreatedAt).ToList(),
        };

        var pageSize = filters.PageSize > 0 ? filters.PageSize : 12;
        var page = filters.Page > 0 ? filters.Page : 1;
        var totalCount = results.Count;
        var items = results.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PagedResult<Ad>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public List<string> GetPublishedCities() => _dbContext.Ads
        .Where(a => a.Status == AdStatus.PUBLISHED && a.City != null && a.City != "")
        .Select(a => a.City!)
        .Distinct()
        .OrderBy(c => c)
        .ToList();

    public List<string> GetPublishedRegions() => _dbContext.Ads
        .Where(a => a.Status == AdStatus.PUBLISHED && a.Region != null && a.Region != "")
        .Select(a => a.Region!)
        .Distinct()
        .OrderBy(r => r)
        .ToList();

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
