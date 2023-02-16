using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MVCIdentity.Models;


namespace MVCIdentity.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
    
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
    
    [AllowAnonymous]
    public IActionResult Register()
    {
        var resturnUrl = HttpContext.Request.GetDisplayUrl().ToString();
        return Redirect($"https://localhost:5001/Account/Register?ReturnUrl={resturnUrl}");
    }
}