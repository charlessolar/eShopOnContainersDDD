using Aggregates;
using NServiceBus;
using System.Collections.Generic;
using Aggregates.Contracts;
using Infrastructure.Commands;
using System;

namespace eShop
{
    public class Mutator : IMutate
    {
        public IMutating MutateIncoming(IMutating mutating)
        {
            return mutating;
        }
        public IMutating MutateOutgoing(IMutating mutating)
        {
            if (!(mutating.Message is StampedCommand) || (mutating.Message as StampedCommand).Stamp != 0) return mutating;

            (mutating.Message as StampedCommand).Stamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            return mutating;
        }
    }
}
