using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket.Entities.Item
{
    public class Item : Aggregates.Entity<Item, State, Basket>
    {
        private Item() { }

        public void Add()
        {
            Apply<Events.ItemAdded>(x =>
            {
                x.BasketId = Parent.Id;
                x.ProductId = Id;
            });
        }

        public void UpdateQuantity(long quantity)
        {
            Apply<Events.QuantityUpdated>(x =>
            {
                x.BasketId = Parent.Id;
                x.ProductId = Id;
                x.Quantity = quantity;
            });
        }

        public void Remove()
        {
            Apply<Events.ItemRemoved>(x =>
            {
                x.BasketId = Parent.Id;
                x.ProductId = Id;
            });
        }
    }
}
