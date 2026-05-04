using Carsure.Models;

namespace Carsure.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        if (dbContext.Ads.Any())
            return;

        dbContext.Ads.AddRange(
            new Ad
            {
                Title = "2019 Toyota Corolla",
                Description = "Well maintained, low mileage.",
                Price = 15000,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Ad
            {
                Title = "2021 Ford Focus",
                Description = "One owner, full service history.",
                Price = 18500,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Ad
            {
                Title = "2018 Honda Civic",
                Description = "Excellent condition, recently serviced.",
                Price = 17000,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Ad
            {
                Title = "2020 Tesla Model 3",
                Description = "Like new, autopilot included.",
                Price = 35000,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        );

        dbContext.SaveChanges();
    }
}