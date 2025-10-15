using System.Diagnostics;
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

        // GET: /Home/Login?role=Lecturer (role param optional)
        public IActionResult Login(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            if (!string.IsNullOrEmpty(username) && Users.Accounts.TryGetValue(username, out var acc))
            {
                if (acc.Password == password && acc.Role == role)
                {
                    HttpContext.Session.SetString("Role", role);
                    HttpContext.Session.SetString("Username", username);

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
    }
}
