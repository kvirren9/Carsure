using Microsoft.AspNetCore.Mvc;
using Carsure.Services;

namespace Carsure.Controllers;

public class AdController : Controller
{
    private readonly AdService _adService;

    public AdController(AdService adService)
    {
        _adService = adService;
    }

    public IActionResult Index()
    {
        var ads = _adService.GetAds();
        return View(ads);
    }

    public IActionResult Details(int id)
    {
        var ad = _adService.GetAdById(id);

        if (ad is null)
        {
            return NotFound();
        }

        return View(ad);
    }
}
