using log4net;
using Quartz;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: zhen_di_shi_ri_le_gou_le.DIRegister(typeof(MyWebApp.Jobs.SimpleJob2))]

namespace MyWebApp.Jobs
{
    public partial class SimpleJob2 : IJob, zhen_di_shi_ri_le_gou_le.IDepInject
    {
        [zhen_di_shi_ri_le_gou_le.DInject] IServiceProvider _serviceProvider;

        private static readonly ILog log = LogManager.GetLogger(typeof(SimpleJob2));

        public async Task Execute(IJobExecutionContext context)
        {
            JobKey jobKey = context.JobDetail.Key;

            log.InfoFormat("SimpleJob2 says: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));
            //Console.WriteLine("SimpleJob2 says: {0} executing at {1}", jobKey, DateTime.Now.ToString("r"));

            await Task.CompletedTask;
        }
    }
}
