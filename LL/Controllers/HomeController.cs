using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var a = HttpContext.User.Claims.Select(d => new { d.Type, d.Value }).ToList();
            var a1 = a[0].Value;
            var a2 = a[1].Value;

            var b = HttpContext.User.Identity;
            return View();
        }
    }
}