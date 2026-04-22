using Carsure.Data;
using Carsure.Models;

namespace Carsure.Services;

public class AdService
{
    private readonly InMemoryAdRepository _adRepository;

    public AdService(InMemoryAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public IReadOnlyList<Ad> GetAds() => _adRepository.GetAll();

    public Ad? GetAdById(int id) => _adRepository.GetById(id);

    public void CreateAd(Ad ad)
    {
        ad.CreatedAt = DateTime.UtcNow;
        _adRepository.Add(ad);
    }

    public void UpdateAd(Ad ad) => _adRepository.Update(ad);

    public void DeleteAd(int id) => _adRepository.Delete(id);
}
