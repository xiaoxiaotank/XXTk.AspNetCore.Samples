using XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs.Jobs;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs;

/// <summary>
/// 后台作业参数帮助类
/// </summary>
public static class BackgroundJobArgsHelper
{
    /// <summary>
    /// 通过作业类型获取作业参数类型
    /// </summary>
    /// <param name="jobType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Type GetJobArgsType(Type jobType)
    {
        foreach (var @interface in jobType.GetInterfaces())
        {
            if (!@interface.IsGenericType)
            {
                continue;
            }

            if (@interface.GetGenericTypeDefinition() != typeof(IBackgroundJob<>))
            {
                continue;
            }

            var genericArgs = @interface.GetGenericArguments();
            if (genericArgs.Length != 1)
            {
                continue;
            }

            return genericArgs[0];
        }

        throw new Exception($"Could not find type of the job args. " +
            $"Ensure that given type implements the {typeof(IBackgroundJob<>).AssemblyQualifiedName} interface. " +
            $"Given job type: {jobType.AssemblyQualifiedName}");
    }
}

