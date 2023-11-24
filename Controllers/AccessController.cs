using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using Hangman_App.Models;

namespace Hangman_App.Controllers
{
    [Route("[controller]/[action]")]
    public class AccessController : Controller
    {
        private readonly ILogger _logger;

        public AccessController(ILogger<AccessController> logger)
        {
            _logger = logger;
        }

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            // Redirect to main menu if already logged in
            ClaimsPrincipal claim = HttpContext.User;
            if (claim != null)
                if (claim.Identity.IsAuthenticated)
                    return RedirectToAction("Menu", "Game");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account account)
        {
            // TODO: Query database to see if user exists
            if (new Random().NextInt64() % 2 == 0)
            {
                List<Claim> claims = new List<Claim>(){ new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties() { AllowRefresh = false, IsPersistent = account.StayLoggedIn };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                // TODO: Log message that user has logged in successfully
                return RedirectToAction("Menu", "Controller");
            }

            ViewData["ErrorMessage"] = "Failed to login, please try again";
            return View();
        }
        #endregion

        #region Registration
        [HttpGet]
        public IActionResult Register()
        {
            // Redirect to main menu if already logged in
            ClaimsPrincipal claim = HttpContext.User;
            if (claim != null)
                if (claim.Identity.IsAuthenticated)
                    return RedirectToAction("Menu", "Game");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Account account)
        {
            // TODO: Add user to database

            return View();
        }

        #endregion
    }
}
