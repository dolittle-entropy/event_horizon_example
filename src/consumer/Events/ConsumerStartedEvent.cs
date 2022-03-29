using Dolittle.SDK.Events;

namespace Consumer.Events;

[EventType("5019c558-c1f4-4438-8860-48470dd649f0")]
public record ConsumerStartedEvent(string Timestamp);
