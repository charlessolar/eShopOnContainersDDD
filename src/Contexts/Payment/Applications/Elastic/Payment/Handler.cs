using Aggregates;
using Aggregates.Application;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Payment.Payment
{
    public class Handler :
        IHandleQueries<Queries.BuyerPayments>,
        IHandleQueries<Queries.Payments>,
        IHandleMessages<Events.Charged>,
        IHandleMessages<Events.Settled>,
        IHandleMessages<Events.Canceled>
    {
        public async Task Handle(Queries.Payments query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            var results = await ctx.Uow().Query<Models.PaymentIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Queries.BuyerPayments query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            builder.Add("UserName", query.UserName.ToString(), Operation.Equal);

            var results = await ctx.Uow().Query<Models.PaymentIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Events.Charged e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.Uow().Get<Ordering.Buyer.Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            var order = await ctx.Uow().Get<Ordering.Order.Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var method = await ctx.Uow().Get<Ordering.Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            var model = new Models.PaymentIndex
            {
                Id = e.PaymentId,
                UserName = e.UserName,
                GivenName = buyer.GivenName,
                Status = Status.Submitted.Value,
                StatusDescription = Status.Submitted.Description,
                OrderId = e.OrderId,
                Reference = "",
                TotalPayment = order.Total,
                PaymentMethodCardholder = method.CardholderName,
                PaymentMethodMethod = Ordering.Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).Value,
                Created = e.Stamp,
                Updated = e.Stamp,
            };

            await ctx.Uow().Add(model.Id, model).ConfigureAwait(false);
        }
        public async Task Handle(Events.Settled e, IMessageHandlerContext ctx)
        {
            var method = await ctx.Uow().Get<Models.PaymentIndex>(e.PaymentId).ConfigureAwait(false);
            method.Status = Status.Settled.Value;
            method.StatusDescription = Status.Settled.Description;

            await ctx.Uow().Update(method.Id, method).ConfigureAwait(false);
        }
        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var method = await ctx.Uow().Get<Models.PaymentIndex>(e.PaymentId).ConfigureAwait(false);
            method.Status = Status.Cancelled.Value;
            method.StatusDescription = Status.Cancelled.Description;

            await ctx.Uow().Update(method.Id, method).ConfigureAwait(false);
        }
    }
}
