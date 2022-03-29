using Dolittle.SDK.Events;
using Dolittle.SDK.Events.Handling;

namespace Consumer.Bootstrapping;

[EventHandler(
    eventHandlerId: "50148974-4a86-436e-9585-3c952ddf57f2",
    inScope: "90e0d0db-ac7d-4815-92d3-1298f9d326bb",
    partitioned: true
)]
public class PublicEventsHandler
{
    public void Handle(PublicProducerStartedEvent evt, EventContext context)
    {
        Console.WriteLine(
            $@"{DateTime.UtcNow} - handling event {evt.GetType().Name} sent at {
                context.Occurred.ToLocalTime():s}"
        );
    }
}
