using NServiceBus.Logging;
using NServiceBus.Pipeline;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Extensions;

namespace Infrastructure.Logging
{
    public class LogIncomingMessageBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            Log.Logger.For<LogIncomingMessageBehavior>()
                .DebugEvent("Incoming", "Message {MessageId} Message headers:\n{@MessageHeaders}\n{@Message}",
                    context.MessageId, context.MessageHeaders,
                    context.Message.Instance);

            return next();
        }
    }
    public class LogIncomingMessageRegistration : RegisterStep
    {
        public LogIncomingMessageRegistration() : base(
            stepId: "LogIncomingMessage",
            behavior: typeof(LogIncomingMessageBehavior),
            description: "Logs incoming messages")
        {
            InsertAfter("LogContextProvider");
        }
    }
}
