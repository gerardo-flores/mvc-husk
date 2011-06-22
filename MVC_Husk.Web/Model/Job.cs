//
//  Copyright Info
//

using System.Threading.Tasks;
using System.Web;

namespace MVC_Husk.Model
{
    public class Job
    {
        TaskFactory _factory;

        public Job()
        {
            _factory = HttpRuntime.Cache.Get("BackgroundTaskScheduler") as TaskFactory;
        }

        public string Name { get; set; }

        public void Test()
        {

        }
    }
}