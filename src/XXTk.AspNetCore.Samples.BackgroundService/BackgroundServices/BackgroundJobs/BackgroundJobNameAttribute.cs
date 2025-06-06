using System.Diagnostics.CodeAnalysis;

namespace XXTk.AspNetCore.Samples.BackgroundService.BackgroundServices.BackgroundJobs;

/// <summary>
/// 后台作业名称特性
/// 加到作业类上以指定作业名称，默认为类的全名
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class BackgroundJobNameAttribute : Attribute, IBackgroundJobNameProvider
{
    public string Name { get; }

    public BackgroundJobNameAttribute([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }

    public static string GetName<TJobArgs>()
    {
        return GetName(typeof(TJobArgs));
    }

    public static string GetName([NotNull] Type jobArgsType)
    {
        Check.NotNull(jobArgsType, nameof(jobArgsType));

        return (jobArgsType
            .GetCustomAttributes(true)
            .OfType<IBackgroundJobNameProvider>()
            .FirstOrDefault()
            ?.Name
        ?? jobArgsType.FullName)!;
    }
}
