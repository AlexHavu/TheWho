using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Tipalti.TheWho.Jobs
{
    public abstract class BaseJob : IJob
    {
        private readonly ILogger _log;

        public BaseJob(
            ILogger log
            )
        {
            _log = log;
        }

        protected abstract Task ExecuteJobAsync(IJobExecutionContext context);

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await ExecuteJobAsync(context);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An error occurred while executing job {jobName}", context.JobDetail.Key);

                throw new JobExecutionException(ex);
            }
        }
    }
}
