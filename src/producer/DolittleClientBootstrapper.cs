using System.Reflection;
using Dolittle.SDK;
using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Events;

namespace Producer;

/**
 * <summary>
 * Bootstrapping the dolittle client
 * </summary>
 */
public static class DolittleClientBootstrapper
{
    static readonly Assembly _thisAssembly = typeof(DolittleClientBootstrapper).Assembly;

    /**
     * <summary>
     * Given a configuration, return a dolittle client with events, event-handlers and projections
     * from this assembly registered
     * </summary>
     * <param name="configuration">
     * configuration. must contain the keys microservice, dolittle:runtime:port and dolittle:runtime:host
     * </param>
     * <returns>the build dolittle client</returns>
     */
    public static Client ConfigureClient(this IConfiguration configuration)
    {
        var microservice = configuration["microservice"];
        var runtimeHost = configuration["dolittle:runtime:host"];
        var runtimePort = ushort.Parse(configuration["dolittle:runtime:port"]);

        return Client
            .ForMicroservice(new Guid(microservice))
            .WithRuntimeOn(runtimeHost, runtimePort)
            .WithEventTypes(b => b.Register<StartedEvent>())
            // .WithEventTypes(b => b.RegisterAllFrom(_thisAssembly))
            // .WithEventHandlers(b => b.RegisterAllFrom(_thisAssembly))
            // .WithProjections(b => b.RegisterAllFrom(_thisAssembly))
            .Build();
    }
}

[EventType("f799e77a-6592-4a72-b1fc-c2c7106ee468")]
public record StartedEvent(string Timestamp);

[AggregateRoot("7f009911-c812-49a0-9357-a2bc4ac9f0d5")]
public class ApplicationAggreageteRoot : AggregateRoot
{
    public ApplicationAggreageteRoot(EventSourceId eventSourceId) : base(eventSourceId)
    {
    }

    public void Start(string timestamp)
    {
        Apply(new StartedEvent(timestamp));
    }
}