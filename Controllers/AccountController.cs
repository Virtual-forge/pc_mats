using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pc_mats.Data; // your DbContext namespace
using pc_mats.Models;
using System.Linq;

namespace pc_mats.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Simple check if email already exists
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("", "Email already registered.");
                    return View(user);
                }

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(string email, string password)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserType", user.UserType.ToString()); // store type
                return RedirectToAction("Index", "Home");
            }


            ModelState.AddModelError("", "Invalid email or password.");
    return View();
}

public IActionResult Logout()
{
    HttpContext.Session.Clear(); // Remove all session data
    return RedirectToAction("Index", "Home");
}

    }
}
