using ClinicApplication.Models;
using ClinicApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ClinicApplication.Controllers
{
    public class AccountController : Controller
    {
        // 初始化 DB Service(動態資料庫)
        private readonly ClinicService _clinicService;

        public AccountController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet("account/login/default")]
        public IActionResult DefaultLoginPage()
        {
            ViewBag.ConnectionName = "DefaultConnection";
            return View("Login");
        }

        [HttpGet("account/login/sunny")]
        public IActionResult SunnyLoginPage()
        {
            ViewBag.ConnectionName = "SunnyConnection";
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                var connectionName = User.FindFirst(ClaimTypes.Role)?.Value;
                if (connectionName == "DefaultConnection")
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("DefaultLoginPage", "Account");
                } 
                else if (connectionName == "SunnyConnection")
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("SunnyLoginPage", "Account");
                }
                else
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("DefaultLoginPage", "Account");
                }
            }
            else
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("DefaultLoginPage", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string connectionName)
        {
            // Validate the user's credentials
            var customer = await _clinicService.ValidateCustomerAsync(connectionName, model.Username, model.Password);

            if (customer != null)
            {
                // Set the user's authentication cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, connectionName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                var principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(principal);

                // Redirect to the home page
                return RedirectToAction("ClinicHome", "Home", new { customerId = customer.Id, connectionName = connectionName });
            }
            else
            {
                // Redirect back to the login page with an error message
                ModelState.AddModelError("", "帳號或密碼有誤，請重新輸入!");
                TempData["ErrorMessage"] = "帳號或密碼有誤，請重新輸入!";
                if(connectionName == "DefaultConnection")
                {
                    return RedirectToAction("DefaultLoginPage", "Account");
                } 
                else
                {
                    return RedirectToAction("SunnyLoginPage", "Account");
                }
            }
        }
    }
}
