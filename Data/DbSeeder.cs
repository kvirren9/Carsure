using Carsure.Models;

namespace Carsure.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        if (dbContext.Ads.Any())
            return;

        var car1 = new Car { RegNumber = "ABC123", Brand = "Toyota", Model = "Corolla", Year = 2019, MileAge = "45000", FuelType = "Petrol", Transmission = "Automatic", Price = 15000, Description = "Well maintained, low mileage." };
        var car2 = new Car { RegNumber = "DEF456", Brand = "Ford", Model = "Focus", Year = 2021, MileAge = "22000", FuelType = "Petrol", Transmission = "Manual", Price = 18500, Description = "One owner, full service history." };
        var car3 = new Car { RegNumber = "GHI789", Brand = "Honda", Model = "Civic", Year = 2018, MileAge = "60000", FuelType = "Petrol", Transmission = "Manual", Price = 17000, Description = "Excellent condition, recently serviced." };
        var car4 = new Car { RegNumber = "JKL012", Brand = "Tesla", Model = "Model 3", Year = 2020, MileAge = "30000", FuelType = "Electric", Transmission = "Automatic", Price = 35000, Description = "Like new, autopilot included." };

        dbContext.Cars.AddRange(car1, car2, car3, car4);
        dbContext.SaveChanges();

        dbContext.Ads.AddRange(
            new Ad { Title = "2019 Toyota Corolla", Description = "Well maintained, low mileage.", Price = 15000, CreatedAt = DateTime.UtcNow.AddDays(-5), CarId = car1.CarId },
            new Ad { Title = "2021 Ford Focus", Description = "One owner, full service history.", Price = 18500, CreatedAt = DateTime.UtcNow.AddDays(-2), CarId = car2.CarId },
            new Ad { Title = "2018 Honda Civic", Description = "Excellent condition, recently serviced.", Price = 17000, CreatedAt = DateTime.UtcNow.AddDays(-10), CarId = car3.CarId },
            new Ad { Title = "2020 Tesla Model 3", Description = "Like new, autopilot included.", Price = 35000, CreatedAt = DateTime.UtcNow.AddDays(-1), CarId = car4.CarId }
        );

        dbContext.SaveChanges();
    }
}