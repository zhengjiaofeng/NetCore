using LLBLL.IService;
using LLBLL.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LL.Controllers
{
    public class HomeController : Controller
    {
        private IUsersService iUsersService;
        /// <summary>
        /// 加密组件
        /// </summary>
        private readonly IDataProtector protector;
        public HomeController(IUsersService _iUsersService, IDataProtectionProvider _provider)
        {
            iUsersService = _iUsersService;

            #region .net core 保护组件

            protector = _provider.CreateProtector("users_Protector");
            //解密
            var ss = "CfDJ8AtfLGrOSnNOoXdGbxHGy3J64gbZWE7TajcjcfjLjqag9-9z2e41C72HokijShZ0l3SZ4YcSAEN4QrQ07TiYi1RrY5lmqkX4R54nQyJ6RjmrqixxYI_kbEy8jhwFGgyOHA";
            //解密
            var result = protector.Unprotect(ss);
            #endregion
        }

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

        #region  JwtBearer 数据保护组件
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetData()
        {
            List<Users> userList = new List<Users>();
            userList = iUsersService.GetUsers();

            var result = userList.Select(d => new { Id = protector.Protect(d.Id.ToString()), UserName = d.UserName }).ToList();
            return Json(result);
        }

        #endregion

        public IActionResult Error()
        {
            return View();
        }
    }
}