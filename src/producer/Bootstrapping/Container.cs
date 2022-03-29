using dolittleExecutionContext = Dolittle.SDK.Execution.ExecutionContext;

namespace Producer.Bootstrapping;

public class Container : Dolittle.SDK.DependencyInversion.IContainer
{
    readonly IServiceProvider _provider;

    public Container(IServiceProvider provider)
    {
        _provider = provider;
    }

    public object Get(Type service, dolittleExecutionContext context)
    {
        SingleTenantExecutionContextManager.Set(context);
        return _provider.GetRequiredService(service);
    }

    public T Get<T>(dolittleExecutionContext context) where T : class
    {
        SingleTenantExecutionContextManager.Set(context);
        return _provider.GetRequiredService<T>();
    }
}
