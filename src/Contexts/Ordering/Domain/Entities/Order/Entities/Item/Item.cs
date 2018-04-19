using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order.Entities.Item
{
    public class Item : Aggregates.Entity<Item, State, Order>
    {
        private Item() { }

        public void Add(Catalog.Product.State product, decimal quantity)
        {
            Apply<Events.Added>(x =>
            {
                x.OrderId = Parent.Id;
                x.ItemId = Id;
                x.ProductId = product.Id;
                x.Quantity = quantity;
            });
        }

        public void ChangeQuantity(decimal quantity)
        {
            Apply<Events.QuantityChanged>(x =>
            {
                x.OrderId = Parent.Id;
                x.ItemId = Id;
                x.Quantity = quantity;
            });
        }
        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.OrderId = Parent.Id;
                x.ItemId = Id;
            });
        }
    }
}
