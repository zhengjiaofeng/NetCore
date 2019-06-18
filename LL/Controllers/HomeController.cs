﻿using LL.Models.ComomModel;
using LLBLL.IService;
using LLBLL.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
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
        private readonly IOptions<CookieSetting> cookiesetting;
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<AccountController> logger;
        public HomeController(IOptions<CookieSetting> _cookiesetting, IUsersService _iUsersService, IDataProtectionProvider _provider, ILogger<AccountController> _logger)
        {
            try
            {
                cookiesetting = _cookiesetting;
                iUsersService = _iUsersService;
                logger = _logger;
                #region .net core 保护组件
                protector = _provider.CreateProtector("users_Protector");

            }

            catch (Exception ex)
            {
                logger.LogError("HomeController ex" + ex.ToString());
            }

            #endregion
        }

        /// <summary>
        /// Authorize--身份验证标识
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = "LLCoreCookie")]
        public IActionResult Index()
        {

            try
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
            }

            catch (Exception ex)
            {
                logger.LogError("Home/Index ex:" + ex.ToString());
            }
            return View();
        }

        #region  JwtBearer 数据保护组件
        //Jwt验证
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = "LLCoreCookie")]
        //多个身份验证
        [Authorize(Policy = "LL_Jwt")]
        public IActionResult GetData()
        {
            try
            {
                List<Users> userList = new List<Users>();
                userList = iUsersService.GetUsers();

                var result = userList.Select(d => new { Id = protector.Protect(d.Id.ToString()), UserName = d.UserName }).ToList();
                //解密
                var unss = result.FirstOrDefault().Id;
                var usresult = protector.Unprotect(unss.ToString());
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.LogError("GetData ex:" + ex.ToString());
            }

            return Json("");
        }

        #endregion

        public IActionResult Error()
        {
            return View();
        }
    }
}