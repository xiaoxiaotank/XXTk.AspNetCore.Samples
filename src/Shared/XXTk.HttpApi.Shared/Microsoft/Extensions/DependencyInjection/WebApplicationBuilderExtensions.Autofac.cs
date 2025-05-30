using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class WebApplicationBuilderExtensions
{
    public static IHostBuilder UseAppAutofacServiceProviderFactory(
        this WebApplicationBuilder builder,
        List<Type> registerTypes,
        Action<ContainerBuilder>? configurationAction = null)
    {
        // 将 Ioc 容器替换为 Autofac
        return builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            // 记得为特定类型支持属性注入
            containerBuilder.RegisterDefault(builder.Services, registerTypes);

            configurationAction?.Invoke(containerBuilder);
        }));
    }
}
