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

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        var isAdmin = UserController.IsAdmin(HttpContext.Session);

        if (userId is null)
            return RedirectToAction("Login", "User");

        if (!_adService.CanModifyAd(id, userId.Value, isAdmin))
            return Forbid();

        _adService.DeleteAd(id);
        TempData["Success"] = "Annonsen har tagits bort.";
        return RedirectToAction("Index");
    }
}
