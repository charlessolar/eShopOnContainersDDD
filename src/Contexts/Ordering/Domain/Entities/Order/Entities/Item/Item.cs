using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Entities.Item
{
    public class Item : Aggregates.Entity<Item, State, Order>
    {
        private Item() { }

        public void Add(long quantity)
        {
            Apply<Events.Added>(x =>
            {
                x.OrderId = Parent.Id;
                x.ProductId = Id;
                x.Quantity = quantity;
            });
        }

        public void OverridePrice(long price)
        {
            Apply<Events.PriceOverridden>(x =>
            {
                x.OrderId = Parent.Id;
                x.ProductId = Id;
                x.Price = price;
            });
        }
        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.OrderId = Parent.Id;
                x.ProductId = Id;
            });
        }
    }
}
