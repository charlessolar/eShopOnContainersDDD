using Aggregates.Messages;
using FluentValidation;
using Infrastructure.Extensions;
using NServiceBus.Pipeline;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Validation
{
    public class FluentValidationBehaviour : Behavior<IIncomingLogicalMessageContext>
    {
        private static readonly object Lock = new object();
        private static readonly HashSet<Type> NotValidated = new HashSet<Type>();

        public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            var messageType = context.Message.MessageType;

            if (!(context.Message.Instance is ICommand) || NotValidated.Contains(messageType))
            {
                await next().ConfigureAwait(false);
                return;
            }

            var factory = context.Builder.Build<IValidatorFactory>();

            var validators = GetBaseTypes(messageType).Concat(new[] { messageType })
                .Where(type => typeof(IMessage).IsAssignableFrom(type))
                .Where(type => type != typeof(IMessage))
                .Select(type => factory.GetValidator(type))
                .Where(validator => validator != null);

            var validationResults = await validators
                .SelectAsync(validator => validator.ValidateAsync(context.Message.Instance))
                .ConfigureAwait(false);

            if (validationResults == null || !validationResults.Any())
            {
                lock (Lock)
                {
                    // 2 commands of the same type received at the same time.  Just check
                    if (!NotValidated.Contains(context.Message.MessageType))
                        NotValidated.Add(context.Message.MessageType);
                }
                await next().ConfigureAwait(false);
                return;
            }
            if (validationResults.Any(x => !x.IsValid))
            {
                Log.Logger.For<FluentValidationBehaviour>().WarnEvent("Validation", "Message {MessageId} has failed validation\n{@Failures}\n{@Message}", context.MessageId, validationResults.SelectMany(x => x.Errors).Select(x => $"{x.PropertyName}: {x.ErrorMessage}"), context.Message.Instance);
                throw new ValidationException(validationResults.SelectMany(x => x.Errors));
            }
            await next().ConfigureAwait(false);
        }
        public IEnumerable<Type> GetBaseTypes(Type type)
        {
            if (type.BaseType == null) return type.GetInterfaces();

            return Enumerable.Repeat(type.BaseType, 1)
                             .Concat(type.GetInterfaces())
                             .Concat(type.GetInterfaces().SelectMany<Type, Type>(GetBaseTypes))
                             .Concat(GetBaseTypes(type.BaseType));
        }
    }
    public class FluentValidationRegistration : RegisterStep
    {
        public FluentValidationRegistration() : base(
            stepId: "FluentValidation",
            behavior: typeof(FluentValidationBehaviour),
            description: "Runs fluent validation on incoming commands")
        {
            InsertAfter("LocalMessageUnpack");
            InsertAfterIfExists("CommandAcceptor");
        }
    }
}
