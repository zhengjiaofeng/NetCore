using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UEditor.Core;

namespace LL.Controllers
{
    public class UeditorController : Controller
    {
        private readonly UEditorService ueditorService;

        public UeditorController(UEditorService _ueditorService)
        {
            ueditorService = _ueditorService;

        }

        public ContentResult Upload()
        {
            var response = ueditorService.UploadAndGetResponse(HttpContext);
            return Content(response.Result, response.ContentType);
        }
    }
}