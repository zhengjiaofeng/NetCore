using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LL.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Authorize--身份验证标识
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Index()
        {
            #region  //获取登录信息1 Claims

            //获取登录信息1

            var a = HttpContext.User.Claims.Select(d => new { d.Type, d.Value }).ToList();
            var a1 = a[0].Value;
            var a2 = a[1].Value;
            #endregion

            #region 获取登录信息2 @User.Identity.Name
            /*
             * 要使用 外部申明属性ClaimTypes，可使用 @User.Identity.Name 获取信息
             */
            #endregion
            //是否登录标识
            bool isLogin = HttpContext.User.Identity.IsAuthenticated;
            string type = User.Identity.AuthenticationType; //验证方式
            return View();
        }
        [HttpGet]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetData()
        {
            return Json("GetData");
        }
    }
}