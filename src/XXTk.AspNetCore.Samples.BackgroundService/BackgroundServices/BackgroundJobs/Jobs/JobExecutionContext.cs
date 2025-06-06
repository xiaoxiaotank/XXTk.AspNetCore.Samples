namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

public class JobExecutionContext
{
    public JobExecutionContext(
        IServiceProvider serviceProvider,
        Type jobType,
        object jobArgs,
        CancellationToken cancellationToken = default)
    {
        ServiceProvider = serviceProvider;
        JobType = jobType;
        JobArgs = jobArgs;
        CancellationToken = cancellationToken;
    }

    public IServiceProvider ServiceProvider { get; }

    public Type JobType { get; }

    public object JobArgs { get; }

    public CancellationToken CancellationToken { get; }
}
