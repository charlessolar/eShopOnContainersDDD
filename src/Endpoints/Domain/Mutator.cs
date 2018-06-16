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
        private readonly Aggregates.UnitOfWork.IDomain _uow;


        public Mutator(Aggregates.UnitOfWork.IDomain uow)
        {
            _uow = uow;
        }

        public IMutating MutateIncoming(IMutating mutating)
        {

            return mutating;
        }

        public IMutating MutateOutgoing(IMutating mutating)
        {
            // Make sure UserId and Stamp are transfer from message to message
            if (mutating.Message is StampedCommand)
            {
                if ((mutating.Message as StampedCommand).Stamp != 0) return mutating;

                if (_uow.CurrentMessage is StampedCommand)
                {
                    var command = _uow.CurrentMessage as StampedCommand;
                    ((StampedCommand)mutating.Message).Stamp = command.Stamp;
                }
                else if (_uow.CurrentMessage is IStampedEvent)
                {
                    var @event = _uow.CurrentMessage as IStampedEvent;
                    ((StampedCommand)mutating.Message).Stamp = @event.Stamp;
                }
                else if((mutating.Message as StampedCommand).Stamp == 0)
                {
                    (mutating.Message as StampedCommand).Stamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
            }
            else if (mutating.Message is IStampedEvent)
            {
                if ((mutating.Message as IStampedEvent).Stamp != 0) return mutating;

                if (_uow.CurrentMessage is StampedCommand)
                {
                    var command = _uow.CurrentMessage as StampedCommand;
                    ((IStampedEvent)mutating.Message).Stamp = command.Stamp;
                }
                else if (_uow.CurrentMessage is IStampedEvent)
                {
                    var @event = _uow.CurrentMessage as IStampedEvent;
                    ((IStampedEvent)mutating.Message).Stamp = @event.Stamp;
                }
                else if ((mutating.Message as IStampedEvent).Stamp == 0)
                {
                    (mutating.Message as IStampedEvent).Stamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                }
            }

            return mutating;
        }
    }
}
