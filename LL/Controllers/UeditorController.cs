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
        private UEditorService ue;

        public UeditorController(UEditorService _ue)
        {
            ue = _ue;

        }
        public IActionResult Index()
        {
            return View();
        }

        public void UpLoad()
        {
            ue.Upload(HttpContext);
        }
    }
}