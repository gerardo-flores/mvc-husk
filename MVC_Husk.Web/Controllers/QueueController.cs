//
//  Copyright Info
//

using System.Web.Mvc;
using MVC_Husk.Infrastructure.BackgroundJobs;
using MVC_Husk.Infrastructure.Logging;
using MVC_Husk.Infrastructure.IdStore;
using MVC_Husk.Model;

namespace MVC_Husk.Controllers
{
    public class QueueController : ApplicationController
    {
        private IBackgroundJobManager _jobManager;
        private Jobs _jobs;

        public QueueController(ILogger logger, IBackgroundJobManager manager, IIdStore store)
            : base(store, logger)
        {
            _jobManager = manager;
            _jobs = new Jobs();
        }

        [Authorize]
        public ActionResult Index()
        {
            var jobs = _jobs.All();

            JobsViewModel model = new JobsViewModel
            {
                Jobs = jobs
            };

            ViewBag.Message = "All the jobs in the queue";

            return View(model);
        }

        [Authorize]
        public ActionResult LongRunning()
        {
            _jobManager.LongRunningJob(CurrentUser);

            ViewBag.Message = "A long running task has been added to the queue";

            return View();
        }
    }
}
