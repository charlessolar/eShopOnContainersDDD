using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket.Entities.Item
{
    public class Item : Aggregates.Entity<Item, State, Basket>
    {
        private Item() { }

        public void Add(Catalog.Product.State product, decimal quantity)
        {
            Apply<Events.ItemAdded>(x =>
            {
                x.BasketId = Parent.Id;
                x.ItemId = Id;
                x.ProductId = product.Id;
                x.Quantity = quantity;
            });
        }

        public void UpdateQuantity(decimal quantity)
        {
            Apply<Events.QuantityUpdated>(x =>
            {
                x.BasketId = Parent.Id;
                x.ItemId = Id;
                x.Quantity = quantity;
            });
        }

        public void Remove()
        {
            Apply<Events.ItemRemoved>(x =>
            {
                x.BasketId = Parent.Id;
                x.ItemId = Id;
            });
        }
    }
}
