using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Carsure.Models;
using Carsure.Services;

namespace Carsure.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AdService _adService;

    public HomeController(ILogger<HomeController> logger, AdService adService)
    {
        _logger = logger;
        _adService = adService;
    }

    public IActionResult Index()
    {
        var latestAds = _adService
            .GetAds()
            .Take(4)
            .ToList();

        return View(latestAds);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
