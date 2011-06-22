using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;

namespace MVC_Husk.Models
{   

    public class JobModel
    {
        private TaskFactory factory = HttpRuntime.Cache.Get("BackgroundTaskScheduler") as TaskFactory;
        
        public string Name { get; set; }

        public void test()
        {
            
        }
        
    }

}