using Microsoft.Extensions.DependencyInjection;

namespace System;

public interface ITransientCachedServiceProvider : ICachedServiceProviderBase, ITransientDependency
{
}