using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantBookingMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestaurantBookingMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        public AccountController(HttpClient client)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri("https://localhost:7037/");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Login", loginUser);

            if(!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorContent);
                Console.WriteLine(errorContent);
                return View(loginUser);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Token);

            var claims = jwtToken.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = jwtToken.ValidTo
            });

            HttpContext.Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = jwtToken.ValidTo
            });

            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Login", "Account");
        }

    }
}
