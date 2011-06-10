using System.Web.Mvc;
using Template.Infrastructure.Logging;
using Template.Model;

namespace Template.Controllers
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
//            var user = table.Single(2);
            // Gerardo, you can also do a lot of other different queries, I'm just doing a 
            // simple "All" because I have three people in the database.
            var users = table.All();

            var viewModel = new UsersViewModel
            {
                People = users
            };
           
            ViewBag.Message = "Welcome!";

            return View(viewModel);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
