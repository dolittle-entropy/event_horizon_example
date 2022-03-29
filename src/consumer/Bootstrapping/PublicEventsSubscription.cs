using Dolittle.SDK.EventHorizon;
using Dolittle.SDK.Events;

namespace Consumer.Bootstrapping;

public static class PublicEventsSubscription
{
    public static void SubscribeTheSingleTenantToThePublicStream(this SubscriptionsBuilder eventHorizons)
    {
         // hard-coded instead of read from event-horizons.json
        var consumerTenant = new Guid("8cfd0dbc-99e5-43d4-9cb3-3d46d240b06b");

        var producerMicroservice = new Guid("5a4a2115-48cf-4055-8b8f-aac0bde47b7b");
        var producerTenant = new Guid("8cfd0dbc-99e5-43d4-9cb3-3d46d240b06b");
        var producerStream = new Guid("2d58d78f-f1ba-4469-86b3-7b89f8018290");
        var scope = new Guid("90e0d0db-ac7d-4815-92d3-1298f9d326bb");

        eventHorizons
            .ForTenant(
                consumerTenant,
                subscribe => subscribe
                    .FromProducerMicroservice(producerMicroservice)
                    .FromProducerTenant(producerTenant)
                    .FromProducerStream(producerStream)
                    .FromProducerPartition(PartitionId.Unspecified)
                    .ToScope(scope)
            );
    }
}
