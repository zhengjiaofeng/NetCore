using LL.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LL.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model == null)
            {
                return Redirect("/Account/Index");
            }
            var claims = new List<Claim>
                {
                    new Claim("username", model.UserName),
                    new Claim("role", "Member")
                };

            //用户标识
            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "LLCoreCookie")),new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(20) });
            return RedirectToAction("Index", "Home");

        }
    }
}