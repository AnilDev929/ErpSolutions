
using ERP_SOLUTIONS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_SOLUTIONS.Controllers
{
    [AllowAnonymous]  // Make login accessible to all
    public class AccountController :Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAccountService _accountService;
        public AccountController(IHttpClientFactory clientFactory, IAccountService accountService)
        {
            _clientFactory = clientFactory;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        // POST: Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Username, string Password, string Role)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ViewBag.Error = "Username and Password are required";
                return View();
            }
            
            var result = await _accountService.LoginAsync(Username, Password, Role);

            if (result.Success)
            {
                int userid = 1; // Replace with actual user ID
                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),           // User.Identity.Name
                    new Claim(ClaimTypes.Email, "rout.anil@gmail.com"),
                    new Claim(ClaimTypes.Role, Role),
                    new Claim(ClaimTypes.NameIdentifier, userid.ToString()) // store UserID
                };

                // Create identity and principal
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in user (await is important!)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Redirect to Dashboard
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = result.Message;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        // GET: Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
