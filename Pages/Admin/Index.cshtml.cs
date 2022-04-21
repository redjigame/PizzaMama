using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pizza_mama.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public bool DisplayInvalidAccountMessage = false;

        IConfiguration configuration;
        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Admin/Pizzas");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string username, string password, string ReturnUrl)
        {
            if (username == "admin")
            {
                DisplayInvalidAccountMessage = false;
                var claims = new List<Claim>{new Claim(ClaimTypes.Name, username)};
                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(ReturnUrl == null ? "/Admin/Pizzas" : ReturnUrl);
            }

            DisplayInvalidAccountMessage = true;
            return Page();
        }

        public async Task<IActionResult> OnGetLogOut()
        {
            var authSection = configuration.GetSection("Auth");

            string adminLogin = "AdminLogin";
            string adminPassword = "AdminPassword";

            await HttpContext.SignOutAsync();
            return Redirect("Admin");
        }
    }
}
