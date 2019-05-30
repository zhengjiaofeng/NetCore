using LL.Common;
using LL.Models.ComomModel;
using LL.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LL.Controllers
{
    public class AccountController : Controller
    {
        private IOptions<LLSetting> settings;
        private string rsaPrivateKey = "";
        private string rsaPublicKey = "";
        public AccountController(IOptions<LLSetting> _settings)
        {
            settings = _settings;
            rsaPrivateKey = settings.Value.RsaPrivateKey;
            rsaPublicKey = settings.Value.RsaPublicKey;
        }
        public IActionResult Index()
        {
            #region rsa 加密
            RsaHelep rsa1 = new RsaHelep();
            var s1 = RsaHelep.RSAEncrypt("123", rsaPublicKey);
            var result1 = RsaHelep.RSADecrypt(s1, rsaPrivateKey);
            #endregion

            ViewData["rsaPublicKey"] = rsaPublicKey;
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
            ResponseResult<string> result = new ResponseResult<string>();
            try
            {
                if (model == null)
                {
                    result.isSucess = false;
                    result.msg = "数据异常！";
                    return Json(result);
                }
                #region 验证
                if (string.IsNullOrEmpty(model.UserName))
                {
                    result.isSucess = false;
                    result.msg = "用户名不能为空！";
                    return Json(result);
                }
                if (string.IsNullOrEmpty(model.PassWord))
                {
                    result.isSucess = false;
                    result.msg = "密码不能为空！";
                    return Json(result);
                }
                var decryptPW = RsaHelep.RSADecrypt(model.PassWord, rsaPrivateKey);
                #endregion

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
                result.isSucess = true;
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    result.msg = ReturnUrl;
                }
                else
                {
                    result.msg = "/Home/Index";
                }


            }
            catch (Exception ex)
            {
                result.isSucess = false;
                result.msg = "系统异常，请重新提交！";
            }

            return Json(result);

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