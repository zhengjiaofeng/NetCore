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
            //var claims = new List<Claim>
            //    {
            //        new Claim("UserName", model.UserName),
            //        new Claim("Age","18")
            //    };

            ////用户标识
            //await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "LLCoreCookie")), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(20) });

            //用户标识
            //ClaimTypes 外部申明属性
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, model.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));

            await HttpContext.SignInAsync("LLCoreCookie1", new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(20) });

            return RedirectToAction("Index", "Home");

        }
    }
}