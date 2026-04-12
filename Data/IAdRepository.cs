using Carsure.Models;

namespace Carsure.Data;

public interface IAdRepository
{
    IReadOnlyList<Ad> GetAll();
    Ad? GetById(int id);
}