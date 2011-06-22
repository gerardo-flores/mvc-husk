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
            _logger.LogInfo("Logging this");

            var table = new Users();
            var users = table.All();

            var viewModel = new UsersViewModel
            {
                People = users
            };
           
            if (ViewBag.Message == null)
                ViewBag.Message = "Welcome!";

            return View(viewModel);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
