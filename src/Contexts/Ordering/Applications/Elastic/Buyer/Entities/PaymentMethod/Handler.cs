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
            if (query.Id.HasValue)
            {
                var result = await ctx.UoW().Get<Models.PaymentMethod>(query.Id.Value).ConfigureAwait(false);

                await ctx.Result(new[] { result }, 1, 0).ConfigureAwait(false);
                return;
            }

            var builder = new QueryBuilder();
            builder.Add("UserName", query.UserName.ToString(), Operation.Equal);
            if (!string.IsNullOrEmpty(query.Term))
            {
                var group = builder.Grouped(Group.Any);
                group.Add("Alias", query.Term, Operation.Autocomplete);
                group.Add("CardholderName", query.Term, Operation.Autocomplete);
                group.Add("CardType", query.Term, Operation.Autocomplete);
            }
            var results = await ctx.UoW().Query<Models.PaymentMethod>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }

        public Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var model = new Models.PaymentMethod
            {
                Id = e.PaymentMethodId,
                UserName = e.UserName,
                Alias = e.Alias,
                CardholderName = e.CardholderName,
                CardNumber = e.CardNumber,
                Expiration = e.Expiration,
                SecurityNumber = e.SecurityNumber,
                CardType = e.CardType.Value
            };

            return ctx.UoW().Add(e.PaymentMethodId, model);
        }

        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.PaymentMethod>(e.PaymentMethodId);
        }
    }
}
