using Dolittle.SDK;
using Dolittle.SDK.Events.Handling;

namespace Consumer.Bootstrapping;

/**
 * <summary>
 * Registration of services with the dependency injection container
 * </summary>
 */
public static class ServicesRegistration
{
    /**
     * <summary>
     * registers classes as services for their interfaces and as themselves. also registers the
     * dolittle client singleton service and performs the first call to the execution context manager
     * so it gets the configurations from which it determines the tenant id
     * </summary>
     * <param name="services"></param>
     * <param name="dolittleClient"></param>
     */
    public static void RegisterServices(
        this IServiceCollection services, Client dolittleClient
    )
    {
        // IFoo -> Foo
        services.Scan(select =>
            select
                .FromEntryAssembly()
                .AddClasses()
                .AsMatchingInterface()
        );

        // register event handlers
        services.Scan(select =>
            select
                .FromEntryAssembly()
                .AddClasses(eventHandler => eventHandler.WithAttribute<EventHandlerAttribute>())
                .AsSelf()
                .WithTransientLifetime()
        );

        services.AddSingleton(dolittleClient);

        services.AddTransient(serviceProvider =>
            SingleTenantExecutionContextManager.GetSingleTenant(serviceProvider)
        );
    }
}
