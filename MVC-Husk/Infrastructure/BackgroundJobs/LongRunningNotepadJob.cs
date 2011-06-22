using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MVC_Husk.Infrastructure.BackgroundJobs
{
    public class LongRunningNotepadJob : IJob
    {
        public void Execute(JobExecutionContext context)
        {
            Task taskC = new Task(() => { Process proc = Process.Start("notepad.exe"); proc.WaitForExit(); });
            taskC.Start();
        }
    }
}