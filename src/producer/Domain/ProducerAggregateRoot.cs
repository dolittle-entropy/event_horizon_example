using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Events;
using Producer.Events;

namespace Producer.Domain;

[AggregateRoot("7f009911-c812-49a0-9357-a2bc4ac9f0d5")]
public class ProducerAggregateRoot : AggregateRoot
{
    public ProducerAggregateRoot(EventSourceId eventSourceId) : base(eventSourceId)
    {
    }

    public void Start(string timestamp)
    {
        Apply(new StartedEvent(timestamp));

        ApplyPublic(new PublicProducerStartedEvent(timestamp));
    }
}