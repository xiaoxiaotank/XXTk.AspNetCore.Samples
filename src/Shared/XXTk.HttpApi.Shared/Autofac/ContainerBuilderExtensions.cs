using Microsoft.AspNetCore.Mvc;

namespace Autofac;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterDefault(
        this ContainerBuilder builder,
        IServiceCollection services,
        List<Type> registerTypes)
    {
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
           .SelectMany(assembly => assembly.GetExportedTypes());

        var controllerTypes = allTypes
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract)
                .ToArray();

        // 配置所有控制器支持属性注入
        builder.RegisterTypes(controllerTypes).PropertiesAutowired();

        // 注册指定类型，使其支持属性注入
        if (!registerTypes.IsNullOrEmpty())
        {
            builder.RegisterSourceTypes(services, [.. registerTypes]);
        }

        return builder;
    }

    /// <summary>
    /// 从服务源中注册指定类型
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sources"></param>
    /// <param name="registerTypes"></param>
    /// <returns></returns>
    private static ContainerBuilder RegisterSourceTypes(this ContainerBuilder builder, IEnumerable<ServiceDescriptor> sources, params Type[] registerTypes)
    {
        var serviceDescriptors = sources
            .Where(s => registerTypes.Any(rt => rt.IsAssignableFrom(s.ServiceType)));

        foreach (var descriptor in serviceDescriptors)
        {
            var implementationType = descriptor.ImplementationType ??
                (descriptor.ServiceType.IsClass && !descriptor.ServiceType.IsAbstract ? descriptor.ServiceType : null);
            if (implementationType != null)
            {
                // 配置所有服务支持属性注入

                if (implementationType.IsGenericTypeDefinition)
                {
                    builder.RegisterGeneric(implementationType)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifetime)
                        .PropertiesAutowired();
                }
                else
                {
                    builder.RegisterType(implementationType)
                        .As(descriptor.ServiceType)
                        .ConfigureLifecycle(descriptor.Lifetime)
                        .PropertiesAutowired();
                }
            }
        }

        return builder;
    }
}