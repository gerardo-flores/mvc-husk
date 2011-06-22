//
//  Copyright Info
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.Reflection;
using Ninject;
using MVC_Husk.Infrastructure.Logging;
using QuartzNetWebConsole;

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public class BackgroundJobManager : IBackgroundJobManager
    {
        private IScheduler _scheduler;
        private MVC_Husk.Infrastructure.Logging.ILogger _logger;

        public BackgroundJobManager(MVC_Husk.Infrastructure.Logging.ILogger logger, IScheduler sched)
        {
            _logger = logger;
            _scheduler = sched;

            if (!_scheduler.IsStarted)
            {
                _scheduler.Start();
                QuartzNetWebConsole.Setup.Scheduler = () => sched;
                QuartzNetWebConsole.Setup.Logger = new MemoryLogger(10000);
            }
        }

        public void LoadDataJob(string model, string path)
        {
            string typeName = "MVC_Husk.Infrastructure.BackgroundJobs.Load" + model + "Job";
            Type temp = Type.GetType(typeName);

            if (temp != null)
            {
                JobDetail details = new JobDetail("Load File for " + model + " at " + path, temp);
                details.JobDataMap["path"] = path;
                details.Description = "Job that loads user data uploaded via the application into the database";
                details.Group = "Data Load";

                Trigger trigger = new SimpleTrigger("QuartzManager LoadDataJob", DateTime.Now);
                trigger.Description = "Trigger immediately";
                trigger.Group = "Immediate";

                _scheduler.ScheduleJob(details, trigger);
            }
            else
            {
                _logger.LogError("There is no " + typeName + "defined");
                throw new NullReferenceException("There is no " + typeName + "defined");
            }

        }

        public void LongRunningJob()
        {
            JobDetail details = new JobDetail("Long Running Task", typeof(LongRunningNotepadJob));
            details.Description = "Long running job";
            details.Group = "External executable job";

            Trigger trigger = new SimpleTrigger("QuartzManager Long Running Task", DateTime.Now);
            trigger.Description = "Trigger immediately";
            trigger.Group = "Immediate";

            _scheduler.ScheduleJob(details, trigger);
        }

    }
}