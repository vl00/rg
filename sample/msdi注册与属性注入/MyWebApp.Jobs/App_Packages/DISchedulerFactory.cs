using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Specialized;

namespace MyWebApp.Jobs
{
    internal class DISchedulerFactory : StdSchedulerFactory
    {
        readonly IJobFactory _jobFactory;

        public DISchedulerFactory(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory ?? throw new ArgumentNullException(nameof(jobFactory));
        }

        public DISchedulerFactory(NameValueCollection props, IJobFactory jobFactory)
            : base(props)
        {
            _jobFactory = jobFactory ?? throw new ArgumentNullException(nameof(jobFactory));
        }

        protected override IScheduler Instantiate(QuartzSchedulerResources rsrcs, QuartzScheduler qs)
        {
            var scheduler = base.Instantiate(rsrcs, qs);
            scheduler.JobFactory = _jobFactory;
            return scheduler;
        }
    }
}
