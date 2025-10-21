using ClaimSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserService _userService;

        public HomeController(UserService userService)
        {
            _userService = userService;
        }

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
        public async Task<IActionResult> Login(string username, string password, string role)
        {
            var isValid = await _userService.ValidateUserAsync(username, password);
            if (isValid)
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user.Role == role)
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