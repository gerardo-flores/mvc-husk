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
using MVC_Husk.Model;

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public class BackgroundJobManager : IBackgroundJobManager
    {
        private IScheduler _scheduler;
        private MVC_Husk.Infrastructure.Logging.ILogger _logger;
        private Jobs _jobs;

        public BackgroundJobManager(MVC_Husk.Infrastructure.Logging.ILogger logger, IScheduler sched)
        {
            _jobs = new Jobs();
            _logger = logger;
            _scheduler = sched;

            if (!_scheduler.IsStarted)
            {
                _scheduler.Start();
                _scheduler.AddGlobalJobListener(new QueueUpdateManager());
                QuartzNetWebConsole.Setup.Scheduler = () => _scheduler;
                QuartzNetWebConsole.Setup.Logger = new MemoryLogger(10000);
            }
        }

        public void LoadDataJob(string model, string path, dynamic user)
        {
            string typeName = "MVC_Husk.Infrastructure.BackgroundJobs.Load" + model + "Job";
            Type modelType = Type.GetType(typeName);

            if (modelType != null)
            {
                string description = "Load File for " + model + " at " + path;
                JobDetail details = new JobDetail(description, modelType);
                details.JobDataMap["path"] = path;
                details.Description = "Job that loads Seasonal Products data uploaded via the application into the database";
                details.Group = "Data Load";

                Trigger trigger = new SimpleTrigger("QuartzManager LoadDataJob", DateTime.Now);
                trigger.Description = "Trigger immediately";
                trigger.Group = "Immediate";

                dynamic result = _jobs.CreateJob(description, 1, DateTime.Now, user.ID);
                details.JobDataMap["QueueID"] = result.JobId;
                _scheduler.ScheduleJob(details, trigger);
            }
            else
            {
                _logger.LogError("There is no " + typeName + "defined");
                throw new NullReferenceException("There is no " + typeName + "defined");
            }

        }

        public void LongRunningJob(dynamic user)
        {
            string description = "Long Running Task";
            JobDetail details = new JobDetail(description, typeof(LongRunningNotepadJob));
            details.Description = "Long running job";
            details.Group = "External executable job";

            Trigger trigger = new SimpleTrigger("QuartzManager Long Running Task", DateTime.Now);
            trigger.Description = "Trigger immediately";
            trigger.Group = "Immediate";

            dynamic result = _jobs.CreateJob(description, 1, DateTime.Now, user.ID);
            details.JobDataMap["QueueID"] = result.JobId;
            _scheduler.ScheduleJob(details, trigger);
        }

    }
}