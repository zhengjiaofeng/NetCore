using LL.Common;
using LL.Models.ComomModel;
using LL.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LL.Controllers
{
    public class AccountController : Controller
    {
        private IOptions<LLSetting> settings;
        public AccountController(IOptions<LLSetting> _settings)
        {
            settings = _settings;
        }
        public IActionResult Index()
        {
            #region rsa 加密

            string rsaPrivateKey = settings.Value.RsaPrivateKey;
            string rsaPublicKey = settings.Value.RsaPublicKey;

            RsaHelep rsa1 = new RsaHelep();
            var s1 = RsaHelep.RSAEncrypt("123", rsaPublicKey);
            var result1 = RsaHelep.RSADecrypt(s1, rsaPrivateKey);
            #endregion
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
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

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }
    }
}