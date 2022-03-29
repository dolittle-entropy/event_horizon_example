using Dolittle.SDK.Events;

namespace Producer.Events;

[EventType("f799e77a-6592-4a72-b1fc-c2c7106ee468")]
public record StartedEvent(string Timestamp);
