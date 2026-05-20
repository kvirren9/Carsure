using Microsoft.AspNetCore.Mvc;
using Carsure.Services;
using Carsure.Models;

namespace Carsure.Controllers;

public class AdController : Controller
{
    private readonly AdService _adService;
    private readonly UserService _userService;
    private readonly IEmailService _emailService;

    public AdController(AdService adService, UserService userService, IEmailService emailService)
    {
        _adService = adService;
        _userService = userService;
        _emailService = emailService;
    }

    public IActionResult Index(AdSearchViewModel filters)
    {
        var pagedResult = _adService.SearchAds(filters);
        ViewBag.Brands = _adService.GetPublishedBrands();
        ViewBag.Models = _adService.GetPublishedModels(filters.Brand);
        ViewBag.Cities = _adService.GetPublishedCities();
        ViewBag.Regions = _adService.GetPublishedRegions();
        ViewBag.Filters = filters;
        ViewBag.PagedResult = pagedResult;
        return View(pagedResult.Items);
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
            City = vm.City.Trim(),
            Region = vm.Region.Trim(),
            Car = car
        };

        _adService.CreateAdWithCar(ad, car);

        TempData["Success"] = "Din annons har skapats!";
        return RedirectToAction("Profile", "User");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        var isAdmin = UserController.IsAdmin(HttpContext.Session);

        if (userId is null)
            return RedirectToAction("Login", "User");

        var ad = _adService.GetAdByIdAnyStatus(id);
        if (ad is null)
            return NotFound();

        if (!_adService.CanModifyAd(id, userId.Value, isAdmin))
            return Forbid();

        var vm = new EditAdViewModel
        {
            Id = ad.Id,
            Title = ad.Title,
            Description = ad.Description,
            Price = ad.Price,
            Status = ad.Status,
            RegNumber = ad.Car.RegNumber,
            Brand = ad.Car.Brand,
            Model = ad.Car.Model,
            Year = ad.Car.Year,
            MileAge = ad.Car.MileAge,
            FuelType = ad.Car.FuelType,
            Transmission = ad.Car.Transmission,
            City = ad.City ?? string.Empty,
            Region = ad.Region ?? string.Empty
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(EditAdViewModel vm)
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        var isAdmin = UserController.IsAdmin(HttpContext.Session);

        if (userId is null)
            return RedirectToAction("Login", "User");

        if (!_adService.CanModifyAd(vm.Id, userId.Value, isAdmin))
            return Forbid();

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
            Id = vm.Id,
            Title = vm.Title,
            Description = vm.Description,
            Price = vm.Price,
            Status = vm.Status,
            City = vm.City.Trim(),
            Region = vm.Region.Trim(),
            Car = car
        };

        _adService.UpdateAd(ad);

        TempData["Success"] = "Annonsen har uppdaterats!";
        return RedirectToAction("Details", new { id = vm.Id });
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

    [HttpGet]
    public IActionResult ContactSeller(int id)
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login", "User");

        var ad = _adService.GetAdById(id);
        if (ad is null)
            return NotFound();

        // Prevent seller from contacting themselves
        if (ad.UserId == userId.Value)
        {
            TempData["Error"] = "Du kan inte kontakta dig själv.";
            return RedirectToAction("Details", new { id });
        }

        var sender = _userService.FindById(userId.Value);
        if (sender is null)
            return RedirectToAction("Login", "User");

        var vm = new ContactSellerViewModel
        {
            AdId = ad.Id,
            AdTitle = ad.Title,
            SellerName = ad.User?.Name ?? string.Empty,
            SellerEmail = ad.User?.Email ?? string.Empty,
            SenderName = sender.Name,
            SenderEmail = sender.Email,
            Subject = $"Fråga om: {ad.Title}"
        };

        return View(vm);
    }

    // POST: /Ad/ContactSeller
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactSeller(ContactSellerViewModel vm)
    {
        var userId = UserController.GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login", "User");

        var ad = _adService.GetAdById(vm.AdId);
        if (ad is null)
            return NotFound();

        // Prevent seller from contacting themselves
        if (ad.UserId == userId.Value)
        {
            TempData["Error"] = "Du kan inte kontakta dig själv.";
            return RedirectToAction("Details", new { id = vm.AdId });
        }

        var sender = _userService.FindById(userId.Value);
        if (sender is null)
            return RedirectToAction("Login", "User");

        // Re-populate read-only fields
        vm.SellerName = ad.User?.Name ?? string.Empty;
        vm.SellerEmail = ad.User?.Email ?? string.Empty;
        vm.AdTitle = ad.Title;
        vm.SenderName = sender.Name;
        vm.SenderEmail = sender.Email;

        if (!ModelState.IsValid)
            return View(vm);

        if (string.IsNullOrWhiteSpace(vm.SellerEmail))
        {
            ModelState.AddModelError(string.Empty, "Säljaren har ingen e-postadress registrerad.");
            return View(vm);
        }

        try
        {
            await _emailService.SendContactEmailAsync(
                toEmail: vm.SellerEmail,
                toName: vm.SellerName,
                fromEmail: vm.SenderEmail,
                fromName: vm.SenderName,
                subject: vm.Subject,
                message: vm.Message,
                adTitle: vm.AdTitle,
                adId: vm.AdId
            );

            TempData["Success"] = "Ditt meddelande har skickats till säljaren!";
            return RedirectToAction("Details", new { id = vm.AdId });
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty,
                "Det gick inte att skicka meddelandet just nu. Försök igen senare.");
            return View(vm);
        }
    }
}
