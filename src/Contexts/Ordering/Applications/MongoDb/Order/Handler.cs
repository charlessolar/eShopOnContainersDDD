using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Order
{
    public class Handler :
        IHandleMessages<Events.Drafted>,
        IHandleMessages<Events.Canceled>,
        IHandleMessages<Events.Confirm>,
        IHandleMessages<Events.Paid>,
        IHandleMessages<Events.Shipped>,
        IHandleMessages<Events.AddressSet>,
        IHandleMessages<Events.PaymentMethodSet>
    {
        public async Task Handle(Events.Drafted e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Basket.Basket.Models.Basket>(e.OrderId)
                .ConfigureAwait(false);
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Buyer.Models.Buyer>(e.BuyerId)
                .ConfigureAwait(false);

            var model = new Models.Order
            {
                Id = e.OrderId,
                BuyerId = buyer.Id,
                BuyerName = buyer.GivenName,
                Status = Status.Submitted.DisplayName,
                StatusDescription = Status.Submitted.Description,
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, model).ConfigureAwait(false);
        }

        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Order>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Cancelled.DisplayName;
            order.StatusDescription = Status.Cancelled.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Confirm e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Order>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Confirmed.DisplayName;
            order.StatusDescription = Status.Confirmed.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Paid e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Order>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Paid.DisplayName;
            order.StatusDescription = Status.Paid.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Shipped e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Order>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Shipped.DisplayName;
            order.StatusDescription = Status.Shipped.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.AddressSet e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Order>(e.OrderId).ConfigureAwait(false);
            var address = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Buyer.Entities.Address.Models.Address>(e.AddressId).ConfigureAwait(false);

            order.AddressId = address.Id;
            order.Address = address.Street;
            order.CityState = $"{address.City}, {address.Street}";
            order.ZipCode = address.ZipCode;
            order.Country = address.Country;

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, order).ConfigureAwait(false);
        }

        public async Task Handle(Events.PaymentMethodSet e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.Order>(e.OrderId).ConfigureAwait(false);
            var method = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            order.PaymentMethod = Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).DisplayName;
            order.PaymentMethodId = method.Id;

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, order).ConfigureAwait(false);
        }
    }
}
