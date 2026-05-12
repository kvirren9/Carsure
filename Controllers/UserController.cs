using Microsoft.AspNetCore.Mvc;
using Carsure.Services;

namespace Carsure.Controllers;

public class UserController : Controller
{
    private readonly UserService _userService;

    public const string SessionKeyUserId = "UserId";
    public const string SessionKeyUserName = "UserName";

    public UserController(UserService userService)
    {
        _userService = userService;
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

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public static int? GetLoggedInUserId(ISession session)
    {
        return session.GetInt32(SessionKeyUserId);
    }
}
