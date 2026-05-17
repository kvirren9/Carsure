using Microsoft.AspNetCore.Mvc;
using Carsure.Services;
using Carsure.Models;

namespace Carsure.Controllers;

public class UserController : Controller
{
    private readonly UserService _userService;
    private readonly AdService _adService;

    public const string SessionKeyUserId = "UserId";
    public const string SessionKeyUserName = "UserName";
    public const string SessionKeyUserRole = "UserRole";

    public UserController(UserService userService, AdService adService)
    {
        _userService = userService;
        _adService = adService;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(string name, string email, string password)
    {
        if (!ModelState.IsValid)
            return View();

        var success = _userService.Register(name, email, password);

        if (!success)
        {
            ModelState.AddModelError("Email", "E-postadressen är redan registrerad.");
            return View();
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _userService.Login(email, password);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Fel e-post eller lösenord.");
            return View();
        }

        HttpContext.Session.SetInt32(SessionKeyUserId, user.Id);
        HttpContext.Session.SetString(SessionKeyUserName, user.Name);
        HttpContext.Session.SetString(SessionKeyUserRole, user.Role.ToString());

        return RedirectToAction("Profile");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Profile()
    {
        var userId = GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login");

        var user = _userService.FindById(userId.Value);
        if (user is null)
            return RedirectToAction("Login");

        var ads = _adService.GetAdsByUser(userId.Value);

        var viewModel = new UserProfileViewModel
        {
            User = user,
            Ads = ads
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult EditProfile()
    {
        var userId = GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login");

        var user = _userService.FindById(userId.Value);
        if (user is null)
            return RedirectToAction("Login");

        var viewModel = new EditProfileViewModel
        {
            Name = user.Name,
            Email = user.Email
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult EditProfile(EditProfileViewModel model)
    {
        var userId = GetLoggedInUserId(HttpContext.Session);
        if (userId is null)
            return RedirectToAction("Login");

        if (!ModelState.IsValid)
            return View(model);

        var success = _userService.UpdateProfile(
            userId.Value,
            model.Name,
            model.Email,
            model.NewPassword
        );

        if (!success)
        {
            ModelState.AddModelError("Email", "E-postadressen används redan av ett annat konto.");
            return View(model);
        }

        HttpContext.Session.SetString(SessionKeyUserName, model.Name);

        TempData["Success"] = "Din profil har uppdaterats!";
        return RedirectToAction("Profile");
    }

    public static int? GetLoggedInUserId(ISession session)
    {
        return session.GetInt32(SessionKeyUserId);
    }

    public static bool IsAdmin(ISession session)
    {
        var role = session.GetString(SessionKeyUserRole);
        return role == UserRole.Admin.ToString();
    }

    public static bool IsLoggedIn(ISession session)
    {
        return session.GetInt32(SessionKeyUserId) != null;
    }
}
