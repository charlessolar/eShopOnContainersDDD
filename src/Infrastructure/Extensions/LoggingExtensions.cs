using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class LoggingExtensions
    {
        public static Lazy<string> EntryAssembly = new Lazy<string>(() =>
        {
            var entry = Assembly.GetEntryAssembly();
            return entry.FullName;
        });

        public static void LogEvent(this ILogger logger, LogEventLevel level, string eventId, string messageTemplate, params object[] propertyValues)
        {
            var props = new[] { eventId }.Concat(propertyValues).ToArray();
            logger.Write(level, "<{EventId:l}> " + messageTemplate, props);
        }
        public static void LogEvent(this ILogger logger, LogEventLevel level, string eventId, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            var props = new[] { eventId }.Concat(propertyValues).ToArray();
            logger.Write(level, ex, "<{EventId:l}> " + messageTemplate, props);
        }

        public static void DebugEvent(this ILogger logger, string eventId, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Debug, eventId, messageTemplate, propertyValues);
        }
        public static void InfoEvent(this ILogger logger, string eventId, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Information, eventId, messageTemplate, propertyValues);
        }
        public static void WarnEvent(this ILogger logger, string eventId, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Warning, eventId, messageTemplate, propertyValues);
        }
        public static void ErrorEvent(this ILogger logger, string eventId, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Error, eventId, messageTemplate, propertyValues);
        }
        public static void FatalEvent(this ILogger logger, string eventId, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Fatal, eventId, messageTemplate, propertyValues);
        }
        public static void DebugEvent(this ILogger logger, string eventId, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Debug, eventId, ex, messageTemplate, propertyValues);
        }
        public static void InfoEvent(this ILogger logger, string eventId, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Information, eventId, ex, messageTemplate, propertyValues);
        }
        public static void WarnEvent(this ILogger logger, string eventId, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Warning, eventId, ex, messageTemplate, propertyValues);
        }
        public static void ErrorEvent(this ILogger logger, string eventId, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Error, eventId, ex, messageTemplate, propertyValues);
        }
        public static void FatalEvent(this ILogger logger, string eventId, Exception ex, string messageTemplate, params object[] propertyValues)
        {
            logger.LogEvent(LogEventLevel.Fatal, eventId, ex, messageTemplate, propertyValues);
        }

        public static ILogger With(this ILogger logger, string propertyName, object value)
        {
            return logger.ForContext(propertyName, value, destructureObjects: true);
        }

        public static ILogger For(this ILogger logger, string value)
        {
            return logger.ForContext("Endpoint", EntryAssembly.Value).ForContext("SourceContext", value);
        }
        public static ILogger For<TClass>(this ILogger logger)
        {
            return logger.ForContext("Endpoint", EntryAssembly.Value).ForContext("SourceContext", typeof(TClass).FullName);
        }
    }
}
