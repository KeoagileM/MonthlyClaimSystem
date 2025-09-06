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

        // GET: Login
        public IActionResult Login(string role)
        {
            ViewBag.Role = role;
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            if (Users.Accounts.ContainsKey(username) &&
                Users.Accounts[username].Password == password &&
                Users.Accounts[username].Role == role)
            {
                HttpContext.Session.SetString("Role", role);

                if (role == "Lecturer") return RedirectToAction("Dashboard", "Lecture");
                if (role == "Coordinator") return RedirectToAction("Dashboard", "Coordinator");
                if (role == "Manager") return RedirectToAction("Dashboard", "Manager");
            }

            ViewBag.Error = "Invalid login.";
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
