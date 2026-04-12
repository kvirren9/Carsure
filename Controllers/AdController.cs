using Microsoft.AspNetCore.Mvc;
using Carsure.Data;
using Carsure.Models;

namespace Carsure.Controllers;

public class AdController : Controller
{
    private readonly IAdRepository _adRepository;

    public AdController(IAdRepository adRepository)
    {
        _adRepository = adRepository;
    }

    public IActionResult Index()
    {
        var ads = _adRepository.GetAll();
        return View(ads);
    }

    public IActionResult Details(int id)
    {
        var ad = _adRepository.GetById(id);

        if (ad is null)
        {
            return NotFound();
        }

        return View(ad);
    }
}
