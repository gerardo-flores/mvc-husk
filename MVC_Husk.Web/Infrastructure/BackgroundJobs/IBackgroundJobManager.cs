//
//  Copyright Info
//

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public interface IBackgroundJobManager
    {
        void LoadDataJob(string model, string path, dynamic user);
        void LongRunningJob(dynamic user);
    }
}
