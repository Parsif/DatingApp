using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    public class AuthContorller : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}