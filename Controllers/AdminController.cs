using Microsoft.AspNetCore.Mvc;
using Carsure.Services;
using Carsure.Models;

namespace Carsure.Controllers;

public class AdminController : Controller
{
    private readonly AdService _adService;
    private readonly UserService _userService;

    public AdminController(AdService adService, UserService userService)
    {
        _adService = adService;
        _userService = userService;
    }

    private IActionResult RequireAdmin()
    {
        if (!UserController.IsAdmin(HttpContext.Session))
            return RedirectToAction("Index", "Home");
        return null!;
    }

    public IActionResult Index()
    {
        var guard = RequireAdmin();
        if (guard != null) return guard;

        var ads = _adService.GetAllAds();
        return View(ads);
    }

    public IActionResult Users()
    {
        var guard = RequireAdmin();
        if (guard != null) return guard;

        var users = _userService.GetAllUsers();
        return View(users);
    }

    [HttpPost]
    public IActionResult DeleteAd(int id)
    {
        var guard = RequireAdmin();
        if (guard != null) return guard;

        _adService.DeleteAd(id);
        TempData["Success"] = "Annonsen har tagits bort.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SetUserRole(int userId, UserRole role)
    {
        var guard = RequireAdmin();
        if (guard != null) return guard;

        var currentUserId = UserController.GetLoggedInUserId(HttpContext.Session);
        if (currentUserId == userId)
        {
            TempData["Error"] = "Du kan inte ändra din egen roll.";
            return RedirectToAction("Users");
        }

        _userService.SetRole(userId, role);
        TempData["Success"] = "Användarrollen har uppdaterats.";
        return RedirectToAction("Users");
    }
}
