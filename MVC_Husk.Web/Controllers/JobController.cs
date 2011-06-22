//
//  Copyright Info
//

using System.Web.Mvc;
using MVC_Husk.Infrastructure.BackgroundJobs;
using MVC_Husk.Infrastructure.Logging;

namespace MVC_Husk.Controllers
{
    public class JobController : Controller
    {
        private ILogger _logger;
        private IBackgroundJobManager _jobManager;

        public JobController(ILogger logger, IBackgroundJobManager manager)
        {
            _logger = logger;
            _jobManager = manager;
        }

        public ActionResult Index()
        {
            //RunNotepad();
            _jobManager.LongRunningJob();

            ViewBag.Message = "A long running task has been added to the queue";

            return View();
        }
    }
}
