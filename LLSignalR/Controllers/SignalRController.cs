using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LLSignalR.Controllers
{
    public class SignalRController : Controller
    {

        public SignalRController()
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}