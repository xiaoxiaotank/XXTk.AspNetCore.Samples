namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppIdentity(this IServiceCollection services) => AddAppIdentityCore(services);

    private static IServiceCollection AddAppIdentityCore(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        return services;
    }
}