using Carsure.Models;

namespace Carsure.Data;

public static class DbSeeder
{
    private sealed record SeedAdData(
        string RegNumber,
        string Brand,
        string Model,
        int Year,
        string Mileage,
        string FuelType,
        string Transmission,
        decimal Price,
        string Title,
        string Description,
        string City,
        string Region,
        int CreatedDaysAgo,
        string[] ImageUrls);

    public static void Seed(ApplicationDbContext dbContext)
    {
        try
        {
            SeedAdminUser(dbContext);
            SeedCarsAndAds(dbContext);
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
        var sampleRegNumbers = new[] { "SWE101", "SWE102", "SWE103", "SWE104", "SWE105", "SWE106" };

        if (dbContext.Cars.Any(c => sampleRegNumbers.Contains(c.RegNumber)))
            return;

        var sampleAds = new[]
        {
            new SeedAdData(
                RegNumber: "SWE101",
                Brand: "Toyota",
                Model: "Corolla Kombi Hybrid",
                Year: 2020,
                Mileage: "58 400",
                FuelType: "Bensin/El",
                Transmission: "Automat",
                Price: 219000m,
                Title: "Toyota Corolla Kombi Hybrid 2020",
                Description: "Branslesnal familjekombi med full servicehistorik, vinterhjul och adaptiv farthallare.",
                City: "Stockholm",
                Region: "Stockholm",
                CreatedDaysAgo: 1,
                ImageUrls: new[]
                {
                    "https://images.unsplash.com/photo-1549924231-f129b911e442?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1583267746897-2cf415887172?auto=format&fit=crop&w=1200&q=80"
                }),
            new SeedAdData(
                RegNumber: "SWE102",
                Brand: "Volvo",
                Model: "XC60",
                Year: 2019,
                Mileage: "73 200",
                FuelType: "Diesel",
                Transmission: "Automat",
                Price: 309000m,
                Title: "Volvo XC60 D4 Momentum 2019",
                Description: "Bekvam SUV med navigation, stolvarme och nyligen besiktad utan anmarkning.",
                City: "Gothenburg",
                Region: "Vastra Gotaland",
                CreatedDaysAgo: 2,
                ImageUrls: new[]
                {
                    "https://images.unsplash.com/photo-1494976388531-d1058494cdd8?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?auto=format&fit=crop&w=1200&q=80"
                }),
            new SeedAdData(
                RegNumber: "SWE103",
                Brand: "Volkswagen",
                Model: "Golf",
                Year: 2018,
                Mileage: "89 500",
                FuelType: "Bensin",
                Transmission: "Manuell",
                Price: 164000m,
                Title: "Volkswagen Golf TSI Comfortline 2018",
                Description: "Palitlig halvkombi med lag forbrukning, Apple CarPlay och komplett servicebok.",
                City: "Malmo",
                Region: "Skane",
                CreatedDaysAgo: 3,
                ImageUrls: new[]
                {
                    "https://images.unsplash.com/photo-1550355291-bbee04a92027?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1503736334956-4c8f8e92946d?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1542282088-fe8426682b8f?auto=format&fit=crop&w=1200&q=80"
                }),
            new SeedAdData(
                RegNumber: "SWE104",
                Brand: "Tesla",
                Model: "Model 3 Long Range",
                Year: 2021,
                Mileage: "41 300",
                FuelType: "El",
                Transmission: "Automat",
                Price: 419000m,
                Title: "Tesla Model 3 Long Range AWD 2021",
                Description: "Lang rackvidd, premiuminterior och aktiv garanti. Hemmaladdare medfoljer.",
                City: "Uppsala",
                Region: "Uppsala",
                CreatedDaysAgo: 4,
                ImageUrls: new[]
                {
                    "https://images.unsplash.com/photo-1617788138017-80ad40651399?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1560958089-b8a1929cea89?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1593941707882-a5bac6861d75?auto=format&fit=crop&w=1200&q=80"
                }),
            new SeedAdData(
                RegNumber: "SWE105",
                Brand: "BMW",
                Model: "320d Touring",
                Year: 2017,
                Mileage: "112 700",
                FuelType: "Diesel",
                Transmission: "Automat",
                Price: 189000m,
                Title: "BMW 320d Touring M Sport 2017",
                Description: "Sportig kombi med M-paket, panoramatak och nyligen bytta bromsbelagg.",
                City: "Linkoping",
                Region: "Ostergotland",
                CreatedDaysAgo: 5,
                ImageUrls: new[]
                {
                    "https://images.unsplash.com/photo-1523983388277-336a66bf9bcd?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1556800572-1b8aeef2c54f?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1525609004556-c46c7d6cf023?auto=format&fit=crop&w=1200&q=80"
                }),
            new SeedAdData(
                RegNumber: "SWE106",
                Brand: "Kia",
                Model: "Ceed SW",
                Year: 2022,
                Mileage: "24 900",
                FuelType: "Bensin",
                Transmission: "Automat",
                Price: 244000m,
                Title: "Kia Ceed SW Advance 2022",
                Description: "Modern kombi med filhallningsassistans, kvarvarande nybilsgaranti och mycket lag miltal.",
                City: "Orebro",
                Region: "Orebro",
                CreatedDaysAgo: 6,
                ImageUrls: new[]
                {
                    "https://images.unsplash.com/photo-1549317661-bd32c8ce0db2?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1493238792000-8113da705763?auto=format&fit=crop&w=1200&q=80",
                    "https://images.unsplash.com/photo-1502877338535-766e1452684a?auto=format&fit=crop&w=1200&q=80"
                })
        };

        var createdAtBase = DateTime.UtcNow;

        foreach (var sample in sampleAds)
        {
            var car = new Car
            {
                RegNumber = sample.RegNumber,
                Brand = sample.Brand,
                Model = sample.Model,
                Year = sample.Year,
                MileAge = sample.Mileage,
                FuelType = sample.FuelType,
                Transmission = sample.Transmission,
                Price = sample.Price,
                Description = sample.Description
            };

            dbContext.Cars.Add(car);

            dbContext.Ads.Add(new Ad
            {
                Title = sample.Title,
                Description = sample.Description,
                ImageUrl = string.Join("\n", sample.ImageUrls),
                Price = sample.Price,
                CreatedAt = createdAtBase.AddDays(-sample.CreatedDaysAgo),
                Status = AdStatus.PUBLISHED,
                City = sample.City,
                Region = sample.Region,
                Car = car
            });
        }

        dbContext.SaveChanges();
    }
}
