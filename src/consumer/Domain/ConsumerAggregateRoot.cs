using Consumer.Events;
using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Events;

namespace Consumer.Domain;

[AggregateRoot("7f009911-c812-49a0-9357-a2bc4ac9f0d5")]
public class ConsumerAggregateRoot : AggregateRoot
{
    public ConsumerAggregateRoot(EventSourceId eventSourceId) : base(eventSourceId)
    {
    }

    public void Start(string timestamp)
    {
        Apply(new ConsumerStartedEvent(timestamp));
    }
}