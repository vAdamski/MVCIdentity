using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCIdentity.Controllers;

public class TestController : Controller
{
    [Authorize]
    public IActionResult Claims()
    {
        return View();
    }
    
    [Authorize (Policy = "admin")]
    public IActionResult PolicyAdmin()
    {
        return View();
    }
    
    [Authorize (Roles = "admin")]
    public IActionResult RoleAdmin()
    {
        return View();
    }
}