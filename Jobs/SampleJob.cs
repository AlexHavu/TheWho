using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;
using Tipalti.TheWho.Logger;

namespace Tipalti.TheWho.Jobs
{
    // This attribute prevents multiple job instances from running in the same time, 
    // ensuring that only a single instance will handle the job event.
    [DisallowConcurrentExecution]
    // This attribute tells Quartz to persist the job data so it can be used for the next job run.
    // If you use this attribute you should always use the DisallowConcurrentExecution attribute with it.
    [PersistJobDataAfterExecution]
    public class SampleJob : BaseJob
    {
        private readonly ILogger<ITheWhoLogger> _logger;

        public SampleJob(ILogger<ITheWhoLogger> logger) : base(logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteJobAsync(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello !");

            return Task.CompletedTask;
        }
    }
}
