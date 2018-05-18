using Aggregates;
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

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.PaymentIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
        public async Task Handle(Queries.BuyerPayments query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            builder.Add("UserName", query.UserName.ToString(), Operation.EQUAL);

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.PaymentIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public async Task Handle(Events.Charged e, IMessageHandlerContext ctx)
        {
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Ordering.Buyer.Models.OrderingBuyerIndex>(e.UserName).ConfigureAwait(false);
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Ordering.Order.Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var method = await ctx.App<Infrastructure.IUnitOfWork>().Get<Ordering.Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            var model = new Models.PaymentIndex
            {
                Id = e.PaymentId,
                UserName = e.UserName,
                GivenName = buyer.GivenName,
                Status = Status.Submitted.DisplayName,
                StatusDescription = Status.Submitted.Description,
                OrderId = e.OrderId,
                Reference = "",
                TotalPayment = order.Total,
                PaymentMethodCardholder = method.CardholderName,
                PaymentMethodMethod = Ordering.Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).DisplayName,
                Created = e.Stamp,
                Updated = e.Stamp,
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(model.Id, model).ConfigureAwait(false);
        }
        public async Task Handle(Events.Settled e, IMessageHandlerContext ctx)
        {
            var method = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.PaymentIndex>(e.PaymentId).ConfigureAwait(false);
            method.Status = Status.Settled.DisplayName;
            method.StatusDescription = Status.Settled.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(method.Id, method).ConfigureAwait(false);
        }
        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var method = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.PaymentIndex>(e.PaymentId).ConfigureAwait(false);
            method.Status = Status.Cancelled.DisplayName;
            method.StatusDescription = Status.Cancelled.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(method.Id, method).ConfigureAwait(false);
        }
    }
}
