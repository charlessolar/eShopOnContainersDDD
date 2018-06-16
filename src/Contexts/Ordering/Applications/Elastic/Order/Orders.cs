using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Queries;

namespace eShop.Ordering.Order
{
    public class Orders :
        IHandleQueries<Queries.Orders>,
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
        public async Task Handle(Queries.Orders query, IMessageHandlerContext ctx)
        {
            var builder = new QueryBuilder();

            if (query.OrderStatus != null)
                builder.Add("Status", query.OrderStatus.Value, Operation.Equal);
            
            if (query.From.HasValue)
                builder.Add("Created", new DateTimeOffset(query.From.Value).ToUnixTimeMilliseconds().ToString(), Operation.GreaterThanOrEqual);
            if (query.To.HasValue)
                builder.Add("Created", new DateTimeOffset(query.To.Value).ToUnixTimeMilliseconds().ToString(), Operation.LessThanOrEqual);
            
            var results = await ctx.UoW().Query<Models.OrderingOrderIndex>(builder.Build())
                .ConfigureAwait(false);

            await ctx.Result(results.Records, results.Total, results.ElapsedMs).ConfigureAwait(false);
        }
    

        public async Task Handle(Events.Drafted e, IMessageHandlerContext ctx)
        {
            var basket = await ctx.UoW().Get<Basket.Basket.Models.BasketIndex>(e.OrderId)
                .ConfigureAwait(false);
            var buyer = await ctx.UoW().Get<Buyer.Models.OrderingBuyerIndex>(e.UserName)
                .ConfigureAwait(false);

            var shipping = await ctx.UoW().Get<Buyer.Entities.Address.Models.Address>(e.ShippingAddressId).ConfigureAwait(false);
            var billing = await ctx.UoW().Get<Buyer.Entities.Address.Models.Address>(e.BillingAddressId).ConfigureAwait(false);
            var method = await ctx.UoW().Get<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            // get all items in basket
            var itemIds = await ctx.Service<Basket.Basket.Entities.Item.Services.ItemsInBasket, string[]>(x => { x.BasketId = e.BasketId; })
                .ConfigureAwait(false);

            var items = await itemIds.SelectAsync(id =>
            {
                return ctx.UoW().Get<Basket.Basket.Entities.Item.Models.BasketItemIndex>(id);
            }).ConfigureAwait(false);

            var model = new Models.OrderingOrderIndex
            {
                Id = e.OrderId,
                UserName = buyer.Id,
                BuyerName = buyer.GivenName,
                Status = Status.Submitted.Value,
                StatusDescription = Status.Submitted.Description,

                ShippingAddressId = shipping.Id,
                ShippingAddress = shipping.Street,
                ShippingCity = shipping.City,
                ShippingState = shipping.State,
                ShippingZipCode = shipping.ZipCode,
                ShippingCountry = shipping.Country,

                BillingAddressId = billing.Id,
                BillingAddress = billing.Street,
                BillingCity = shipping.City,
                BillingState = shipping.State,
                BillingZipCode = billing.ZipCode,
                BillingCountry = billing.Country,

                PaymentMethodId = method.Id,
                PaymentMethod = Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).Value,
                TotalItems = items.Count(),
                SubTotal = items.Sum(x => x.SubTotal),
                TotalQuantity = items.Sum(x => x.Quantity),

                Created = e.Stamp,
                Updated = e.Stamp
            };

            await ctx.UoW().Add(e.OrderId, model).ConfigureAwait(false);
        }

        public async Task Handle(Events.Canceled e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Cancelled.Value;
            order.StatusDescription = Status.Cancelled.Description;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Confirm e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Confirmed.Value;
            order.StatusDescription = Status.Confirmed.Description;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Paid e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Paid.Value;
            order.StatusDescription = Status.Paid.Description;
            order.Updated = e.Stamp;
            order.Paid = true;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.Shipped e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);

            order.Status = Status.Shipped.Value;
            order.StatusDescription = Status.Shipped.Description;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Events.AddressChanged e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrder>(e.OrderId).ConfigureAwait(false);
            var billing = await ctx.UoW()
                .Get<Buyer.Entities.Address.Models.Address>(e.BillingId).ConfigureAwait(false);
            var shipping = await ctx.UoW()
                .Get<Buyer.Entities.Address.Models.Address>(e.ShippingId).ConfigureAwait(false);

            order.ShippingAddressId = shipping.Id;
            order.ShippingAddress = shipping.Street;
            order.ShippingCityState = $"{shipping.City}, {shipping.Street}";
            order.ShippingZipCode = shipping.ZipCode;
            order.ShippingCountry = shipping.Country;

            order.BillingAddressId = billing.Id;
            order.BillingAddress = billing.Street;
            order.BillingCityState = $"{billing.City}, {billing.Street}";
            order.BillingZipCode = billing.ZipCode;
            order.BillingCountry = billing.Country;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }

        public async Task Handle(Events.PaymentMethodChanged e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var method = await ctx.UoW()
                .Get<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(e.PaymentMethodId).ConfigureAwait(false);

            order.PaymentMethod = Buyer.Entities.PaymentMethod.CardType.FromValue(method.CardType).Value;
            order.PaymentMethodId = method.Id;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Added e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var product = await ctx.UoW().Get<Catalog.Product.Models.CatalogProductIndex>(e.ProductId).ConfigureAwait(false);

            order.TotalItems++;
            order.TotalQuantity += e.Quantity;
            order.SubTotal += (e.Quantity * product.Price);
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.PriceOverridden e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var item = await ctx.UoW().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            order.SubTotal -= item.SubTotal;

            item.Price = e.Price;

            order.SubTotal += item.SubTotal;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Item.Events.Removed e, IMessageHandlerContext ctx)
        {
            var order = await ctx.UoW().Get<Models.OrderingOrderIndex>(e.OrderId).ConfigureAwait(false);
            var item = await ctx.UoW().Get<Entities.Item.Models.OrderingOrderItem>(Entities.Item.Handler.ItemIdGenerator(e.OrderId, e.ProductId)).ConfigureAwait(false);

            order.SubTotal -= item.SubTotal;
            order.Updated = e.Stamp;

            await ctx.UoW().Update(e.OrderId, order).ConfigureAwait(false);
        }
    }
}
