using Aggregates.Messages;

namespace Infrastructure.Commands
{
    public class StampedCommand : ICommand
    {
        public long Stamp { get; set; }
    }
}
