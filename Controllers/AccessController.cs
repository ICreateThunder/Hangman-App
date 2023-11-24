using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using Hangman_App.Models;
using Hangman_App.Context;
using System.Security.Cryptography;
using System.Text;
using NuGet.Common;

namespace Hangman_App.Controllers
{
    [Route("[controller]/[action]")]
    public class AccessController : Controller
    {
        private readonly ILogger<AccessController> _logger;
        private readonly AppDbContext _dbContext;

        public AccessController(ILogger<AccessController> logger, AppDbContext context)
        {
            _logger = logger;
            _dbContext = context;
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
            List<Account> accounts = new List<Account>(_dbContext.Accounts.Where(a => a.Username == account.Username));
            SHA512 sha512 = SHA512.Create();
            // ! EXTREMELY INEFFICIENT && INSECURE !
            // CONSIDER MAKING USERNAMES UNIQUE TO BE ABLE TO ONLY CHECK MAX 1 HASH
            // IF MULTIPLE PEOPLE HAVE THE SAME USERNAME EVERY ACCOUNT IS CHECKED AGAINST
            foreach (Account a in accounts)
            {
                byte[] input = Encoding.UTF8.GetBytes(account.Password += a.Salt);
                if (Encoding.UTF8.GetString(sha512.ComputeHash(input)) == a.Password)
                {
                    List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()) };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties() { AllowRefresh = false, IsPersistent = account.StayLoggedIn };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                    // TODO: Log message that user has logged in successfully
                    return RedirectToAction("Menu", "Game");
                }
            }
            sha512.Dispose();

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
            if (account != null)
                if (account.Username != null)
                    if (account.Password != null)
                    {
                        Random random = new Random();
                        Account a = new Account();
                        SHA512 sha512 = SHA512.Create();
                        string saltValue = "";
                        for (int i = 0; i < 13; i++) 
                        {
                            int offset = random.Next(0, 26);
                            saltValue += Convert.ToChar(random.Next(0, 1) == 0 ? (65 + offset) : (97 + offset));
                        }
                        a.Username = account.Username;
                        a.Password = Encoding.UTF8.GetString(sha512.ComputeHash(Encoding.UTF8.GetBytes(account.Password += saltValue)));
                        a.Salt = saltValue;
                        sha512.Dispose();

                        _dbContext.Accounts.Add(a);
                        _dbContext.SaveChanges();

                        return RedirectToAction("Login", "Access");
                    }

            return View();
        }

        #endregion
    }
}
