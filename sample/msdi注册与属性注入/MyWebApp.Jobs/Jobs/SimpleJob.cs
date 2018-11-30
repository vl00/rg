using log4net;
using Quartz;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.Jobs
{
    public class SimpleJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SimpleJob));

        public async Task Execute(IJobExecutionContext context)
        {
            // This job simply prints out its job name and the
            // date and time that it is running
            JobKey jobKey = context.JobDetail.Key;

            log.InfoFormat("SimpleJob says: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));
            //Console.WriteLine("SimpleJob says: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));

            await Task.CompletedTask;
        }
    }
}
