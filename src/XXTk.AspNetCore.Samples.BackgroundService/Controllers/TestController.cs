using Microsoft.AspNetCore.Mvc;
using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Managers;
using XXTk.HttpApi.Shared.Controllers.Abstractions;

namespace XXTk.AspNetCore.Samples.BackgroundService.Controllers;

public class TestController : AppApiControllerBase
{
    [HttpPost("send-job")]
    public Task SendJobAsync([FromServices] IBackgroundJobManager backgroundJobManager)
    {
        return backgroundJobManager.EnqueueAsync(new MyQuartzJobArgs { Name = DateTime.Now.ToString() });
    }
}
