using Dolittle.SDK.Events;

namespace Consumer.Bootstrapping;

/**
 * <summary>
 * this is the consumer-version of the public event. it must correspond to the sent event by
 * the event-type-id and have the same properties
 * </summary>
 * <returns></returns>
 */
[EventType("1edeafc4-ea30-4f03-b9cf-fc78c5183f47")]
public record PublicProducerStartedEvent(string Timestamp);