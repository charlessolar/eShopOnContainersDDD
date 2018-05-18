using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod
{
    public class Handler :
        IHandleQueries<Queries.PaymentMethods>,
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.Removed>
    {
        public async Task Handle(Queries.PaymentMethods query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();
            builder.Add("UserName", query.UserName.ToString(), Operation.EQUAL);

            var results = await ctx.App<Infrastructure.IUnitOfWork>().Query<Models.PaymentMethod>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var model = new Models.PaymentMethod
            {
                Id = e.PaymentMethodId,
                UserName = e.UserName,
                Alias =e.Alias,
                CardholderName=e.CardholderName,
                CardNumber=e.CardNumber,
                Expiration=e.Expiration,
                SecurityNumber = e.SecurityNumber,
                CardType = e.CardType.Value
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.PaymentMethodId, model);
        }

        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.PaymentMethod>(e.PaymentMethodId);
        }
    }
}
