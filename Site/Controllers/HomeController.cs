using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Site.Models;

namespace Site.Controllers {

    [Authorize]
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public async Task<IActionResult> Index() {
            await LogIdentity();
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        public async Task Logout() {
            // clear cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // sign out of Identity provider - this redirects user to Identity provider user interface end-session endpoint, so it can clear it's cookie
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        private async Task LogIdentity() {
            string? identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            Debug.WriteLine($"Identity token: {identityToken}");
            foreach (Claim claim in User.Claims) {
                Debug.WriteLine($"Clime type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}