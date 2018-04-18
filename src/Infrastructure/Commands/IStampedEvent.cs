using Aggregates.Messages;

namespace Infrastructure.Commands
{
    public interface IStampedEvent : IEvent
    {
        long Stamp { get; set; }
    }
}
