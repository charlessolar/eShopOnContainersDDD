using Aggregates;
using NServiceBus;
using System.Collections.Generic;
using Aggregates.Contracts;
using Infrastructure.Commands;

namespace eShop
{
    public class Mutator : IMutate
    {
        private readonly Aggregates.IDomainUnitOfWork _uow;


        public Mutator(Aggregates.IDomainUnitOfWork uow)
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
                if (_uow.CurrentMessage is StampedCommand)
                {
                    var command = _uow.CurrentMessage as StampedCommand;
                    ((StampedCommand)mutating.Message).Stamp = command.Stamp;
                }
                if (_uow.CurrentMessage is IStampedEvent)
                {
                    var @event = _uow.CurrentMessage as IStampedEvent;
                    ((StampedCommand)mutating.Message).Stamp = @event.Stamp;
                }
            }
            else if (mutating.Message is IStampedEvent)
            {
                if (_uow.CurrentMessage is StampedCommand)
                {
                    var command = _uow.CurrentMessage as StampedCommand;
                    ((IStampedEvent)mutating.Message).Stamp = command.Stamp;
                }
                if (_uow.CurrentMessage is IStampedEvent)
                {
                    var @event = _uow.CurrentMessage as IStampedEvent;
                    ((IStampedEvent)mutating.Message).Stamp = @event.Stamp;
                }
            }

            return mutating;
        }
    }
}
