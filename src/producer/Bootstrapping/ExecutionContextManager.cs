using System.Runtime.Serialization;
using Dolittle.SDK.Events.Store;
using Dolittle.SDK.Events.Store.Builders;
using Dolittle.SDK.Tenancy;
using dolittleExecutionContext = Dolittle.SDK.Execution.ExecutionContext;

namespace Producer.Bootstrapping;

/**
 * <summary>
 * The execution context given a single-tenant application
 * </summary>
 */
public class SingleTenantExecutionContextManager
{
    static readonly AsyncLocal<dolittleExecutionContext> Current = new();
    static IConfiguration? _configuration;
    static ILogger<SingleTenantExecutionContextManager>? _logger;
    static TenantId? _tenantId;

    /**
     * <summary>
     * The single tenant-it
     * </summary>
     */
    public static TenantId SingleTenant => _tenantId ??= GetTenantId();

    /**
     * <summary>
     * Get the tenant id from the services (from an IConfiguration). stores the configuration.
     * this must be called at least once for the SingleTenantExecutionContextManager to work.
     * </summary>
     */
    public static TenantId GetSingleTenant(IServiceProvider serviceProvider)
    {
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _logger = serviceProvider.
            GetRequiredService<ILogger<SingleTenantExecutionContextManager>>();
        return SingleTenant;
    }

    public static readonly Func<EventStoreBuilder, IEventStore> ForCurrentTenant =
        _ => _.ForTenant(SingleTenant);

    public static void Set(dolittleExecutionContext context) => Current.Value = context;

    static TenantId GetTenantId()
    {
        var tenantId = _configuration switch
        {
            null => throw new SingleTenantIdIsNotConfigured(
                "Configuration is not available - please call with a service provider at least once"
            ),
            _ => string.IsNullOrEmpty(_configuration["singleTenantId"])
                ? throw new SingleTenantIdIsNotConfigured("singleTenantId was not configured")
                : _configuration["singleTenantId"]
        };

        _logger
            ?.LogInformation(
                "Configured with single tenant id: {tenantId}",
                tenantId
            );

        return tenantId;
    }

    [Serializable]
    internal class SingleTenantIdIsNotConfigured : Exception
    {
        public SingleTenantIdIsNotConfigured()
        {
        }

        public SingleTenantIdIsNotConfigured(string? message) : base(message)
        {
        }

        public SingleTenantIdIsNotConfigured(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SingleTenantIdIsNotConfigured(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}