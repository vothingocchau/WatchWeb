using Microsoft.AspNetCore.Mvc;

namespace WatchWeb.Admin.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
