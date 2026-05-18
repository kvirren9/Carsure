using Microsoft.AspNetCore.Mvc;
using Carsure.Services;
using Carsure.Models;

namespace Carsure.Controllers;

public class AdController : Controller
{
    private readonly AdService _adService;

    public AdController(AdService adService)
    {
        _adService = adService;
    }

    public IActionResult Index(AdSearchViewModel filters)
    {
        var ads = _adService.SearchAds(filters);
        ViewBag.Brands = _adService.GetPublishedBrands();
        ViewBag.Models = _adService.GetPublishedModels(filters.Brand);
        ViewBag.Filters = filters;
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

    [HttpGet]
    public IActionResult Create()
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login", "User");

        return View(new CreateAdViewModel());
    }

    [HttpPost]
    public IActionResult Create(CreateAdViewModel vm)
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login", "User");

        if (!ModelState.IsValid)
            return View(vm);

        var car = new Car
        {
            RegNumber = vm.RegNumber,
            Brand = vm.Brand,
            Model = vm.Model,
            Year = vm.Year,
            MileAge = vm.MileAge,
            FuelType = vm.FuelType,
            Transmission = vm.Transmission,
            Price = vm.Price,
            Description = vm.Description
        };

        var ad = new Ad
        {
            Title = vm.Title,
            Description = vm.Description,
            Price = vm.Price,
            Status = vm.Status,
            ImageUrl = string.Join("\n", vm.GetImageUrls()),
            UserId = userId.Value,
            Car = car
        };

        _adService.CreateAdWithCar(ad, car);

        TempData["Success"] = "Din annons har skapats!";
        return RedirectToAction("Profile", "User");
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
