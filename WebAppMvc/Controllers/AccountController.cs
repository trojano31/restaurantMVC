using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebAppMvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return Content(BCrypt.Net.BCrypt.HashPassword("password"));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(string password)
        {
            var passwordDb = "$2b$10$DeOB2GHXtgpL41wwL02iAeeZ.CwneuRKP5qs3Wo1nGyaK42YHrldG";

            if (!BCrypt.Net.BCrypt.Verify(password, passwordDb))
                return RedirectToAction("Login");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1", CookieAuthenticationDefaults.AuthenticationScheme),
                new Claim(ClaimTypes.GivenName, "Jan", CookieAuthenticationDefaults.AuthenticationScheme),
                new Claim(ClaimTypes.Surname, "Kowalski", CookieAuthenticationDefaults.AuthenticationScheme),
                new Claim(ClaimTypes.Name, "Jan Kowalski", CookieAuthenticationDefaults.AuthenticationScheme),
                new Claim(ClaimTypes.Role, "Admin", CookieAuthenticationDefaults.AuthenticationScheme),
                new Claim(ClaimTypes.Role, "User", CookieAuthenticationDefaults.AuthenticationScheme)
            };

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Surname, ClaimTypes.Role);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(principal, authenticationProperties);

            return RedirectToAction("Index", controllerName: "Home");
        }

        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}