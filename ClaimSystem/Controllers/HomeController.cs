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
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("EmployeeNumber", user.EmployeeNumber);

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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string role, string firstName, string lastName, string employeeNumber)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(employeeNumber))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            if (password.Length < 4)
            {
                ViewBag.Error = "Password must be at least 4 characters long.";
                return View();
            }

            // Check if username already exists
            if (await _userService.UsernameExistsAsync(username))
            {
                ViewBag.Error = "Username already exists. Please choose a different username.";
                return View();
            }

            // Register the user with employee number
            var success = await _userService.RegisterUserAsync(username, password, role, firstName, lastName, employeeNumber);
            if (success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please login with your credentials.";
                return RedirectToAction("Login", new { role = role });
            }
            else
            {
                ViewBag.Error = "Registration failed. Please try again.";
                return View();
            }
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