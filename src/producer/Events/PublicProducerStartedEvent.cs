using Dolittle.SDK.Events;

namespace Producer.Events;

[EventType("1edeafc4-ea30-4f03-b9cf-fc78c5183f47")]
public record PublicProducerStartedEvent(string Timestamp);
