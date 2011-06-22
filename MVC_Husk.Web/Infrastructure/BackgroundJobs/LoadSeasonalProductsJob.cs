//
//  Copyright Info
//

using MVC_Husk.Model;
using Quartz;

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
            new SeasonalProducts().LoadFileData(path);
        }
    }
}