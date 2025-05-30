namespace System;

public class TransientCachedServiceProvider : CachedServiceProviderBase, ITransientCachedServiceProvider
{
    public TransientCachedServiceProvider(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }
}