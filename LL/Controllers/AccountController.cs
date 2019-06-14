using LL.Common;
using LL.Models.ComomModel;
using LL.Models.ViewModels;
using LLBLL.Common.Tool;
using LLBLL.IService;
using LLBLL.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LL.Controllers
{
    public class AccountController : Controller
    {
        #region 服务声明
        private readonly IOptions<LLSetting> settings;
        private readonly IOptions<JWTSetting> jwtsettings;
        private readonly IOptions<CookieSetting> cookiesetting;
        /// <summary>
        /// 公钥
        /// </summary>
        private string rsaPrivateKey = "";

        /// <summary>
        /// 私钥
        /// </summary>
        private string rsaPublicKey = "";
        private IUsersService iUsersService;
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<AccountController> logger;

        #endregion
        public AccountController(IOptions<LLSetting> _settings, IOptions<JWTSetting> _wtsettings, IOptions<CookieSetting> _cookiesetting, IUsersService _iUsersService, ILogger<AccountController> _logger)
        {
            settings = _settings;
            jwtsettings = _wtsettings;
            cookiesetting = _cookiesetting;
            rsaPrivateKey = settings.Value.RsaPrivateKey;
            rsaPublicKey = settings.Value.RsaPublicKey;
            iUsersService = _iUsersService;
            logger = _logger;
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

                #region 登录
                Users users =await iUsersService.GetUserByUserName(model.UserName);

                if (users == null)
                {
                    result.isSucess = false;
                    result.msg = "该用户并未注册！";
                    return Json(result);
                }

                string rsa_dec_pw= RsaHelep.RSADecrypt(model.PassWord, rsaPrivateKey);
                if (users.PassWord.ToUpper() != rsa_dec_pw.ToUpper())
                {
                    result.isSucess = false;
                    result.msg = "密码错误！";
                    return Json(result);
                }


                #region 登录成功 cooKioe token
                #region cookioe 身份
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

                await HttpContext.SignInAsync(cookiesetting.Value.CookieScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(60) });
                #endregion

                #region Token

                //对称秘钥
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsettings.Value.SecretKey));
                //签名证书(秘钥，加密算法)
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actual

                var token = new JwtSecurityToken(
                  issuer: jwtsettings.Value.Issuer,
                  audience: jwtsettings.Value.Audience,
                  expires: DateTime.Now.AddMinutes(65),
                  //签名
                  signingCredentials: creds);


                #endregion
                #endregion


                result.isSucess = true;
                result.token = new JwtSecurityTokenHandler().WriteToken(token);
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    result.msg = ReturnUrl;
                }
                else
                {
                    result.msg = "/Home/Index";
                }
                #endregion



            }
            catch (Exception ex)
            {
                result.isSucess = false;
                result.msg = "系统异常，请重新提交！";
                logger.LogError("ex:" + ex.ToString());
            }

            return Json(result);

        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LoginOut()
        {
            await HttpContext.SignOutAsync(cookiesetting.Value.CookieScheme);
            return RedirectToAction("Index", "Home");

        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}