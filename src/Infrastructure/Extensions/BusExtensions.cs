using Aggregates;
using Aggregates.Messages;
using Infrastructure.Commands;
using Infrastructure.Exceptions;
using Infrastructure.Queries;
using Infrastructure.ServiceStack;
using NServiceBus;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class BusExtensions
    {
        private static readonly TimeSpan TenSeconds = TimeSpan.FromSeconds(10);

        public static Task Result<TResponse>(this IMessageHandlerContext context, TResponse payload, string eTag = "") where TResponse : class
        {
            return context.Reply<Reply>(x =>
            {
                x.ETag = eTag;
                x.Payload = payload;
            });
        }
        public static Task Result<TResponse>(this IMessageHandlerContext context, IEnumerable<TResponse> records, long total, long elapsedMs) where TResponse : class
        {
            return context.Reply<PagedReply>(x =>
            {
                x.Records = records.ToList();
                x.Total = total;
                x.ElapsedMs = elapsedMs;
            });
        }
        public static PagedResponse<TResponse> RequestPaged<TResponse>(this Aggregates.Messages.IMessage message) where TResponse : class
        {
            if (message == null || message is Reject)
            {
                var reject = (Reject)message;
                Log.Logger.Warning("Query was rejected - Message: {0}\n", reject?.Message);
                if (reject != null)
                    throw new QueryRejectedException(reject.Message);
                throw new QueryRejectedException();
            }
            if (message is Error)
            {
                var error = (Error)message;
                Log.Logger.Warning("Query raised an error - Message: {0}", error.Message);
                throw new QueryRejectedException(error.Message);
            }

            var package = (PagedReply)message;
            if (package == null)
                throw new QueryRejectedException($"Unexpected response type: {message.GetType().FullName}");

            return new PagedResponse<TResponse>()
            {
                RoundTripMs = package.ElapsedMs,
                Total = package.Total,
                Records = package.Records.Cast<TResponse>().ToArray()
            };
        }
        public static QueryResponse<TResponse> RequestQuery<TResponse>(this Aggregates.Messages.IMessage message) where TResponse : class
        {
            if (message == null || message is Reject)
            {
                var reject = (Reject)message;
                Log.Logger.Warning("Query was rejected - Message: {0}\n", reject?.Message);
                if (reject != null)
                    throw new QueryRejectedException(reject.Message);
                throw new QueryRejectedException();
            }
            if (message is Error)
            {
                var error = (Error)message;
                Log.Logger.Warning("Query raised an error - Message: {0}", error.Message);
                throw new QueryRejectedException(error.Message);
            }

            var package = (Reply)message;
            if (package == null)
                throw new QueryRejectedException($"Unexpected response type: {message.GetType().FullName}");

            return new QueryResponse<TResponse>()
            {
                RoundTripMs = package.ElapsedMs,
                Payload = package.Payload as TResponse
            };
        }
        public static async Task CommandToDomain<T>(this IMessageSession bus, T message, bool timeout = true) where T : StampedCommand
        {
            var options = new SendOptions();
            options.SetDestination("domain");
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = bus.Request<Aggregates.Messages.IMessage>(message, options);

            if (timeout)
            {
                await Task.WhenAny(
                        Task.Delay(TenSeconds), response)
                    .ConfigureAwait(false);
            }
            else
                await response.ConfigureAwait(false);

            if (!response.IsCompleted)
                throw new CommandTimeoutException("Command timed out");

            response.Result.CommandResponse();
        }
        public static async Task<object> RequestPaged<T, TResponse>(this IMessageSession bus, T message, bool timeout = true) where T : Paged where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination("elastic");
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = bus.Request<PagedReply>(message, options);

            if (timeout)
            {
                await Task.WhenAny(
                        Task.Delay(TenSeconds), response)
                    .ConfigureAwait(false);
            }
            else
                await response.ConfigureAwait(false);

            if (!response.IsCompleted)
                throw new CommandTimeoutException("Request timed out");

            return response.Result.RequestPaged<TResponse>();
        }
        public static async Task<object> RequestQuery<T, TResponse>(this IMessageSession bus, T message, bool timeout = true) where T : Query where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination("mongodb");
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = bus.Request<Reply>(message, options);

            if (timeout)
            {
                await Task.WhenAny(
                        Task.Delay(TenSeconds), response)
                    .ConfigureAwait(false);
            }
            else
                await response.ConfigureAwait(false);

            if (!response.IsCompleted)
                throw new CommandTimeoutException("Request timed out");

            if (response.Result.Payload == null)
                throw new QueryRejectedException("No results for query");
            
            return response.Result.RequestQuery<TResponse>();
        }
    }
}
