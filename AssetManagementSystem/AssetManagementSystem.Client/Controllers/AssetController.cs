using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Client.Controllers;
public class AssetController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
