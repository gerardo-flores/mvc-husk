//
//  Copyright Info
//

using System.Web.Mvc;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Model;

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
            return View();
        }

    }
}
