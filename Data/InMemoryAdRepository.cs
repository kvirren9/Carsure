
// This class acts as a simple in-memory repository for car ads. 
// This should be removed as soon as a more permanent data storage solution is implemented.
using Carsure.Models;

namespace Carsure.Data;

public class InMemoryAdRepository : IAdRepository
{
    private readonly List<Ad> _ads =
    [
        new Ad { Id = 1, Title = "2019 Toyota Corolla", Description = "Well maintained, low mileage.", Price = 15000, CreatedAt = DateTime.Now.AddDays(-5) },
        new Ad { Id = 2, Title = "2021 Ford Focus", Description = "One owner, full service history.", Price = 18500, CreatedAt = DateTime.Now.AddDays(-2) },
        new Ad { Id = 3, Title = "2018 Honda Civic", Description = "Excellent condition, recently serviced.", Price = 17000, CreatedAt = DateTime.Now.AddDays(-10) },
        new Ad { Id = 4, Title = "2020 Tesla Model 3", Description = "Like new, autopilot included.", Price = 35000, CreatedAt = DateTime.Now.AddDays(-1) },
        new Ad { Id = 5, Title = "2017 BMW 3 Series", Description = "Luxury sedan, well maintained.", Price = 22000, CreatedAt = DateTime.Now.AddDays(-7) },
        new Ad { Id = 6, Title = "2016 Audi A4", Description = "Sporty and stylish, great condition.", Price = 20000, CreatedAt = DateTime.Now.AddDays(-3) }
    ];

    public IReadOnlyList<Ad> GetAll()
    {
        return _ads.OrderByDescending(ad => ad.CreatedAt).ToList();
    }

    public Ad? GetById(int id)
    {
        return _ads.FirstOrDefault(ad => ad.Id == id);
    }
}