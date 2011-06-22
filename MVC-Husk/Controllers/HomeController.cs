using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Husk.Infrastructure.Logging;

namespace MVC_Husk.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to MVC Husk";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
