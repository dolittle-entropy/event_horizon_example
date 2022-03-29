using System.Reflection;
using Dolittle.SDK;
using Dolittle.SDK.Events;
using Producer.Events;

namespace Producer.Bootstrapping;

/**
 * <summary>
 * Bootstrapping the dolittle client
 * </summary>
 */
public static class DolittleClientBootstrapper
{
    static readonly Assembly ThisAssembly = typeof(DolittleClientBootstrapper).Assembly;

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

        // stream from event-horizon-consents.json
        var filterId = new Guid("2d58d78f-f1ba-4469-86b3-7b89f8018290");

        return Client
            .ForMicroservice(new Guid(microservice))
            .WithRuntimeOn(runtimeHost, runtimePort)
            // .WithEventTypes(b => b.Register<StartedEvent>())
            .WithEventTypes(b => b.RegisterAllFrom(ThisAssembly))
            // .WithEventHandlers(b => b.RegisterAllFrom(_thisAssembly))
            // .WithProjections(b => b.RegisterAllFrom(_thisAssembly))
            .WithFilters(b => b.CreatePublicFilter(
                filterId,
                filterBuilder => filterBuilder.Handle((evt, ctx) =>
                {
                    Console.WriteLine(
                        $"{DateTime.UtcNow} - filtering event {evt.GetType().Name} to public filter"
                    );
                    return Task.FromResult(
                        new Dolittle.SDK.Events.Filters.PartitionedFilterResult(
                            shouldInclude: true,
                            partitionId: PartitionId.Unspecified // empty guid
                        )
                    );
                })
            ))
            .Build();
    }
}
