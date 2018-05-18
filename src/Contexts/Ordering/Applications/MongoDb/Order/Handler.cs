using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Ordering.Order
{
    public class Handler :
        IHandleMessages<Events.Drafted>,
        IHandleMessages<Events.Canceled>,
        IHandleMessages<Events.Confirm>,
        IHandleMessages<Events.Paid>,
        IHandleMessages<Events.Shipped>,
        IHandleMessages<Events.AddressChanged>,
        IHandleMessages<Events.PaymentMethodChanged>,
        IHandleMessages<Entities.Item.Events.Added>,
        IHandleMessages<Entities.Item.Events.PriceOverridden>,
        IHandleMessages<Entities.Item.Events.Removed>
    {
        public async Task Handle(Events.Drafted e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.App<Infrastructure.IUnitOfWork>().Get<Basket.Basket.Models.Basket>(e.OrderId)
                .ConfigureAwait(false);
            var buyer = await ctx.App<Infrastructure.IUnitOfWork>().Get<Buyer.Models.Buyer>(e.UserName)
                .ConfigureAwait(false);

            var address = await ctx.App<Infrastructure.IUnitOfWork>().Get<Buyer.Entities.Address.Models.Address>(e.AddressId).ConfigureAwait(false);
            var method = await ctx.App<Infrastructure.IUnitOfWork>().Get<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            // get all items in basket
            var itemIds = await ctx.Service<Basket.Basket.Entities.Item.Services.ItemsInBasket, Guid[]>(x => { x.BasketId = e.BasketId; })
                .ConfigureAwait(false);

            var items = await itemIds.SelectAsync(id =>
            {
                return ctx.App<Infrastructure.IUnitOfWork>().Get<Basket.Basket.Entities.Item.Models.BasketItemIndex>(id);
            }).ConfigureAwait(false);

            var model = new Models.OrderingOrder
            {
                Id = e.OrderId,
                UserName = buyer.UserName,
                BuyerName = buyer.GivenName,
                Status = Status.Submitted.DisplayName,
                StatusDescription = Status.Submitted.Description,
                TotalItems = items.Count(),
                AddressId = address.Id,
                Address = address.Street,
                CityState = $"{address.City}, {address.State}",
                ZipCode = address.ZipCode,
                Country = address.Country,
                PaymentMethodId = method.Id,
                PaymentMethod = Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).DisplayName,
                SubTotal = items.Sum(x => x.SubTotal),
                TotalQuantity = items.Sum(x => x.Quantity),
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.OrderId, model).ConfigureAwait(false);
        }

        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Cancelled.DisplayName;
            order.StatusDescription = Status.Cancelled.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Confirm e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Confirmed.DisplayName;
            order.StatusDescription = Status.Confirmed.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Paid e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Paid.DisplayName;
            order.StatusDescription = Status.Paid.Description;
            order.Paid = true;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Shipped e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Shipped.DisplayName;
            order.StatusDescription = Status.Shipped.Description;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.AddressChanged e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);
            var address = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Buyer.Entities.Address.Models.Address>(e.AddressId).ConfigureAwait(false);

            order.AddressId = address.Id;
            order.Address = address.Street;
            order.CityState = $"{address.City}, {address.Street}";
            order.ZipCode = address.ZipCode;
            order.Country = address.Country;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }

        public async Task Handle(Events.PaymentMethodChanged e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);
            var method = await ctx.App<Infrastructure.IUnitOfWork>()
                .Get<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            order.PaymentMethod = Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).DisplayName;
            order.PaymentMethodId = method.Id;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Added e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);
            var product = await ctx.App<Infrastructure.IUnitOfWork>().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            order.TotalItems++;
            order.TotalQuantity += e.Quantity;
            order.SubTotal += (e.Quantity * product.Price);

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.PriceOverridden e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);
            var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            order.SubTotal -= item.SubTotal;

            item.Price = e.Price;

            order.SubTotal += item.SubTotal;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Removed e, IMessageHandlerContext ctx)
        {
            var order = await ctx.App<Infrastructure.IUnitOfWork>().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);
            var item = await ctx.App<Infrastructure.IUnitOfWork>().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            order.SubTotal -= item.SubTotal;

            await ctx.App<Infrastructure.IUnitOfWork>().Update(e.OrderId, order).ConfigureAwait(false);
        }
    }
}
