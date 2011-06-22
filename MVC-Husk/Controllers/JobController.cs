using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
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
        //Test method to run a few NotePad processes
        //static void RunNotepad()
        //{
        //        Task taskC = new Task(() => { Process proc = Process.Start("notepad.exe"); proc.WaitForExit(); });
        //        taskC.Start();
        //        Task taskd = new Task(() => { Process proc = Process.Start("notepad.exe"); proc.WaitForExit(); });
        //        taskd.Start();
        //        Task taske = new Task(() => { Process proc = Process.Start("notepad.exe"); proc.WaitForExit(); });
        //        taske.Start();
        //}

    }
}
