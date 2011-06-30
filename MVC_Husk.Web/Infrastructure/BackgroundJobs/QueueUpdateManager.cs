using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using MVC_Husk.Model;

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public class QueueUpdateManager : IJobListener
    {
        private Jobs _jobs = new Jobs();

        #region IJobListener Members

        public void JobExecutionVetoed(JobExecutionContext context)
        {
            _jobs.Update(new { Status = "Cancelled" }, context.JobDetail.JobDataMap["QueueID"]);

        }

        public void JobToBeExecuted(JobExecutionContext context)
        {
            _jobs.Update(new { Status = "Started" }, context.JobDetail.JobDataMap["QueueID"]);
        }

        public void JobWasExecuted(JobExecutionContext context, JobExecutionException jobException)
        {
            _jobs.Update(new { Status = "Complete" }, context.JobDetail.JobDataMap["QueueID"]);
        }

        public string Name
        {
            get { return "QueueUpdateManager"; }
        }

        #endregion
    }
}