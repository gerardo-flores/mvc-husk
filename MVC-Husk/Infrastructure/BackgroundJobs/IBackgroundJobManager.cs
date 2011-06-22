using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public interface IBackgroundJobManager
    {
        void LoadDataJob(string model, string path);
        void LongRunningJob();
    }
}
