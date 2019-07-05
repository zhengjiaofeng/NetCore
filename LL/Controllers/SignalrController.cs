using LL.Models.ComomModel;
using LL.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace LL.Controllers
{
    public class SignalrController : Controller
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILogger<AccountController> logger;

        private readonly IOptions<ImagePathSetting> imagePathSetting;

        public SignalrController(ILogger<AccountController> _logger, IOptions<ImagePathSetting> _imagePathSetting)
        {
            logger = _logger;
            imagePathSetting = _imagePathSetting;

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

        public IActionResult ImageUpload()
        {
            ResponseResult<string> result = new ResponseResult<string>();
            result.isSucess = false;

            try
            {
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string chatImgPath = imagePathSetting.Value.ChatImg;
                    string uploadPath = Directory.GetCurrentDirectory() + chatImgPath;
                    if (!Directory.Exists(uploadPath))
                    {
                        //创建文件夹
                        Directory.CreateDirectory(uploadPath);
                    }

                    string fileType = Path.GetExtension(files[0].FileName);//文件扩展明
                    string timeName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string fileName = "/" + timeName + fileType;
                    using (var fs = System.IO.File.Create(uploadPath + fileName))
                    {
                        files[0].CopyTo(fs);
                        //Stream 都有 Flush() 方法，
                        //根据官方文档的说法
                        //“使用此方法将所有信息从基础缓冲区移动到其目标或清除缓冲区，或者同时执行这两种操作”
                        fs.Flush();
                    }
                    result.isSucess = true;
                    result.msg = uploadPath + fileName;
                    return Json(result);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(result);
        }
    }
}