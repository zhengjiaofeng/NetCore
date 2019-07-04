using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LL.Controllers
{
    public class SignalrController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<AccountController> logger;

        public SignalrController(ILogger<AccountController> _logger)
        {
            logger = _logger;

        }

        /// <summary>
        /// Authorize--身份验证标识
        /// </summary>
        /// <returns></returns>
       // [Authorize(AuthenticationSchemes = "LLCoreCookie")]
        public IActionResult Index()
        {
            return View();
        }
    }
}