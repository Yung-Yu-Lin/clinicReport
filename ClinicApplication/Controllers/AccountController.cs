using ClinicApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClinicApplication.Controllers
{
    public class AccountController : Controller
    {
        // 初始化 DBContext
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate the user's credentials
            var customer = await _context.Customer
                .FirstOrDefaultAsync(c => c.Id == model.Username && c.WebPassWD == model.Password);

            if (customer != null)
            {
                // Set the user's authentication cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                var principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(principal);

                // Redirect to the home page
                return RedirectToAction("ClinicHome", "Home", new { customerId = customer.Id });
            }
            else
            {
                // Redirect back to the login page with an error message
                ModelState.AddModelError("", "帳號或密碼有誤，請重新輸入!");
                TempData["ErrorMessage"] = "帳號或密碼有誤，請重新輸入!";
                return View();
            }
        }
    }
}
