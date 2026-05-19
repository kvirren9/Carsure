using Carsure.Models;

namespace Carsure.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        try
        {
            SeedAdminUser(dbContext);

            if (!dbContext.Cars.Any())
            {
                SeedCarsAndAds(dbContext);
            }

            BackfillMissingAdImages(dbContext);
            BackfillSampleAdGalleries(dbContext);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeder error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    private static void SeedAdminUser(ApplicationDbContext dbContext)
    {
        const string adminEmail = "admin@carsure.se";

        if (dbContext.Users.Any(u => u.Email == adminEmail))
            return;

        var admin = new User
        {
            Name = "Admin",
            Email = adminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = UserRole.Admin
        };

        dbContext.Users.Add(admin);
        dbContext.SaveChanges();
        Console.WriteLine("Admin user seeded: admin@carsure.se / Admin123!");
    }

    private static void SeedCarsAndAds(ApplicationDbContext dbContext)
    {
        var car1 = new Car { RegNumber = "ABC123", Brand = "Toyota", Model = "Corolla", Year = 2019, MileAge = "45000", FuelType = "Petrol", Transmission = "Automatic", Price = 15000, Description = "Well maintained, low mileage." };
        var car2 = new Car { RegNumber = "DEF456", Brand = "Ford", Model = "Focus", Year = 2021, MileAge = "22000", FuelType = "Petrol", Transmission = "Manual", Price = 18500, Description = "One owner, full service history." };
        var car3 = new Car { RegNumber = "GHI789", Brand = "Honda", Model = "Civic", Year = 2018, MileAge = "60000", FuelType = "Petrol", Transmission = "Manual", Price = 17000, Description = "Excellent condition, recently serviced." };
        var car4 = new Car { RegNumber = "JKL012", Brand = "Tesla", Model = "Model 3", Year = 2020, MileAge = "30000", FuelType = "Electric", Transmission = "Automatic", Price = 35000, Description = "Like new, autopilot included." };

        dbContext.Cars.AddRange(car1, car2, car3, car4);
        dbContext.SaveChanges();

        dbContext.Ads.AddRange(
            new Ad
            {
                Title = "2019 Toyota Corolla",
                Description = "Well maintained, low mileage.",
                ImageUrl = string.Join("\n", new[]
                {
                    "https://images.unsplash.com/photo-1549924231-f129b911e442?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1583267746897-2cf415887172?auto=format&fit=crop&w=1200&q=80"
                }),
                Price = 15000,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                CarId = car1.CarId,
                Status = AdStatus.PUBLISHED
            },
            new Ad
            {
                Title = "2021 Ford Focus",
                Description = "One owner, full service history.",
                ImageUrl = string.Join("\n", new[]
                {
                    "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?auto=format&fit=crop&w=1200&q=80"
                }),
                Price = 18500,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                CarId = car2.CarId,
                Status = AdStatus.PUBLISHED
            },
            new Ad { Title = "2018 Honda Civic", Description = "Excellent condition, recently serviced.", ImageUrl = "https://images.unsplash.com/photo-1550355291-bbee04a92027?auto=format&fit=crop&w=1200&q=80", Price = 17000, CreatedAt = DateTime.UtcNow.AddDays(-10), CarId = car3.CarId, Status = AdStatus.PUBLISHED },
            new Ad { Title = "2020 Tesla Model 3", Description = "Like new, autopilot included.", ImageUrl = "https://images.unsplash.com/photo-1617788138017-80ad40651399?auto=format&fit=crop&w=1200&q=80", Price = 35000, CreatedAt = DateTime.UtcNow.AddDays(-1), CarId = car4.CarId, Status = AdStatus.PUBLISHED }
        );

        dbContext.SaveChanges();
    }

    private static void BackfillMissingAdImages(ApplicationDbContext dbContext)
    {
        var sampleImageUrls = new[]
        {
            "https://images.unsplash.com/photo-1549924231-f129b911e442?auto=format&fit=crop&w=1200&q=80",
            "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&w=1200&q=80",
            "https://images.unsplash.com/photo-1550355291-bbee04a92027?auto=format&fit=crop&w=1200&q=80",
            "https://images.unsplash.com/photo-1617788138017-80ad40651399?auto=format&fit=crop&w=1200&q=80"
        };

        var adsWithoutImages = dbContext.Ads
            .Where(a => string.IsNullOrWhiteSpace(a.ImageUrl))
            .OrderBy(a => a.Id)
            .ToList();

        if (!adsWithoutImages.Any())
            return;

        for (var i = 0; i < adsWithoutImages.Count; i++)
        {
            adsWithoutImages[i].ImageUrl = sampleImageUrls[i % sampleImageUrls.Length];
        }

        dbContext.SaveChanges();
    }

    private static void BackfillSampleAdGalleries(ApplicationDbContext dbContext)
    {
        var sampleGalleries = new Dictionary<string, string[]>
        {
            ["2019 Toyota Corolla"] = new[]
            {
                "https://images.unsplash.com/photo-1549924231-f129b911e442?auto=format&fit=crop&w=1200&q=80",
                "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=1200&q=80",
                "https://images.unsplash.com/photo-1583267746897-2cf415887172?auto=format&fit=crop&w=1200&q=80"
            },
            ["2021 Ford Focus"] = new[]
            {
                "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&w=1200&q=80",
                "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1200&q=80",
                "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?auto=format&fit=crop&w=1200&q=80"
            }
        };

        var adsToUpdate = dbContext.Ads
            .Where(a => sampleGalleries.Keys.Contains(a.Title))
            .ToList();

        if (!adsToUpdate.Any())
            return;

        var changed = false;

        foreach (var ad in adsToUpdate)
        {
            var existingUrls = (ad.ImageUrl ?? string.Empty)
                .Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            foreach (var url in sampleGalleries[ad.Title])
            {
                if (!existingUrls.Contains(url, StringComparer.OrdinalIgnoreCase))
                {
                    existingUrls.Add(url);
                }
            }

            var normalized = existingUrls
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(10)
                .ToList();

            var merged = string.Join("\n", normalized);
            if (!string.Equals(ad.ImageUrl, merged, StringComparison.Ordinal))
            {
                ad.ImageUrl = merged;
                changed = true;
            }
        }

        if (changed)
            dbContext.SaveChanges();
    }
}
