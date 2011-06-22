using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using MVC_Husk.Models;

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public class LoadSeasonalProductsJob : IJob
    {
        public LoadSeasonalProductsJob()
        {
        }

        public void Execute(JobExecutionContext context)
        {
            string path = context.JobDetail.JobDataMap.GetString("path");
            SeasonalProduct.LoadFileData(path);
        }
    }
}