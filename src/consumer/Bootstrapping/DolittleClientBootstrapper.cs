using System.Reflection;
using Dolittle.SDK;
using Dolittle.SDK.Events;

namespace Consumer.Bootstrapping;

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

        // hard-coded instead of read from event-horizons.json
        var consumerTenant = new Guid("8cfd0dbc-99e5-43d4-9cb3-3d46d240b06b");

        var producerMicroservice = new Guid("5a4a2115-48cf-4055-8b8f-aac0bde47b7b");
        var producerTenant = new Guid("8cfd0dbc-99e5-43d4-9cb3-3d46d240b06b");
        var producerStream = new Guid("2d58d78f-f1ba-4469-86b3-7b89f8018290");
        var scope = new Guid("90e0d0db-ac7d-4815-92d3-1298f9d326bb");

        return Client
            .ForMicroservice(new Guid(microservice))
            .WithRuntimeOn(runtimeHost, runtimePort)
            .WithEventTypes(b => b.RegisterAllFrom(_thisAssembly))
            .WithEventHorizons(eventHorizons =>
                eventHorizons
                    .ForTenant(
                        consumerTenant,
                        subscribe => subscribe
                            .FromProducerMicroservice(producerMicroservice)
                            .FromProducerTenant(producerTenant)
                            .FromProducerStream(producerStream)
                            .FromProducerPartition(PartitionId.Unspecified)
                            .ToScope(scope)
                    )
            )
            .WithEventHandlers(b => b.RegisterAllFrom(_thisAssembly))
            // .WithProjections(b => b.RegisterAllFrom(_thisAssembly))
            .Build();
    }
}
