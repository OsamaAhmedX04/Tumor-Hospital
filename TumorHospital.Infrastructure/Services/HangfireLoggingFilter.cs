using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;

namespace TumorHospital.Infrastructure.Services
{
    public class HangfireLoggingFilter : JobFilterAttribute,
        IServerFilter, IApplyStateFilter
    {
        private readonly ILogger<HangfireLoggingFilter> _logger;

        public HangfireLoggingFilter(ILogger<HangfireLoggingFilter> logger)
        {
            _logger = logger;
        }
        public void OnPerforming(PerformingContext context)
        {
            _logger.LogInformation(
                "Hangfire job started: {JobName} | JobId: {JobId}",
                context.BackgroundJob.Job.Type.Name,
                context.BackgroundJob.Id
            );
        }
        public void OnPerformed(PerformedContext context)
        {
            if (context.Exception == null)
            {
                _logger.LogInformation(
                    "Hangfire job completed successfully: {JobName} | JobId: {JobId}",
                    context.BackgroundJob.Job.Type.Name,
                    context.BackgroundJob.Id
                );
            }
            else
            {
                _logger.LogError(
                    context.Exception,
                    "Hangfire job failed: {JobName} | JobId: {JobId}",
                    context.BackgroundJob.Job.Type.Name,
                    context.BackgroundJob.Id
                );
            }
        }
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState is FailedState failed)
            {
                _logger.LogError(
                    failed.Exception,
                    "Hangfire job moved to FAILED state: {JobName} | JobId: {JobId}",
                    context.BackgroundJob.Job.Type.Name,
                    context.BackgroundJob.Id
                );
            }
        }
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}
