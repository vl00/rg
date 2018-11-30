using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;
using System.Globalization;

namespace MyWebApp.Jobs
{
    internal class DIJobFactory : IJobFactory, IDisposable
    {
        readonly IServiceProvider _serviceProvider;

        internal readonly ConcurrentDictionary<object, IDisposable> RunningJobs = new ConcurrentDictionary<object, IDisposable>();

        public DIJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Dispose()
        {
            RunningJobs.Clear();
        }

        public virtual IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            if (bundle == null) throw new ArgumentNullException(nameof(bundle));
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));

            var jobDetail = bundle.JobDetail;
            var nestedScope = _serviceProvider.CreateScope();

            IJob newJob;
            try
            {
                newJob = ResolveJobInstance(nestedScope, jobDetail);

                RunningJobs[newJob] = nestedScope;
            }
            catch (Exception ex)
            {
                nestedScope?.Dispose();
                throw new SchedulerConfigException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to instantiate Job '{0}' of type '{1}'",
                    bundle.JobDetail.Key, bundle.JobDetail.JobType), ex);
            }
            return newJob;
        }

        protected virtual IJob ResolveJobInstance(IServiceScope nestedScope, IJobDetail jobDetail)
        {
            var jobType = jobDetail.JobType;
            return (IJob)nestedScope.ServiceProvider.GetRequiredService(jobType);
        }

        public void ReturnJob(IJob job)
        {
            if (job == null)
                return;

            if (!RunningJobs.TryRemove(job, out var trackingInfo))
            {
                (job as IDisposable)?.Dispose();
            }
            else
            {
                trackingInfo?.Dispose();
            }
        }
    }
}
