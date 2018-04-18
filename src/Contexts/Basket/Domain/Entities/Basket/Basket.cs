using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket
{
    public class Basket : Aggregates.Entity<Basket, State>
    {
        private Basket() { }

        public void Destroy()
        {
            Apply<Events.Destroyed>(x =>
            {
                x.BasketId = Id;
            });
        }
    }
}
