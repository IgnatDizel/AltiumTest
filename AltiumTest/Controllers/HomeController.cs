using Microsoft.AspNetCore.Mvc;

namespace AltiumTest.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}
