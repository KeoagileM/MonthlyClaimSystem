using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            if (!string.IsNullOrEmpty(username) && Users.Accounts.TryGetValue(username, out var user))
            {
                if (user.Password == password && user.Role == role)
                {
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetString("Username", user.Username);

                    return role switch
                    {
                        "Lecturer" => RedirectToAction("Dashboard", "Lecture"),
                        "Coordinator" => RedirectToAction("Dashboard", "Coordinator"),
                        "Manager" => RedirectToAction("Dashboard", "Manager"),
                        _ => RedirectToAction("Index")
                    };
                }
            }

            ViewBag.Error = "Invalid username, password, or role.";
            ViewBag.Role = role;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}