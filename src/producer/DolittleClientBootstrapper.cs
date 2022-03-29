using System.Reflection;
using Dolittle.SDK;

namespace producer;

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
            .ForMicroservice(microservice)
            .WithRuntimeOn(runtimeHost, runtimePort)
            .WithEventTypes(b => b.RegisterAllFrom(_thisAssembly))
            .WithEventHandlers(b => b.RegisterAllFrom(_thisAssembly))
            .WithProjections(b => b.RegisterAllFrom(_thisAssembly))
            .Build();
    }
}