using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Basket
{
    public class Basket : Aggregates.Entity<Basket, State, Setup>
    {
        private Basket() { }

        public void Seeded()
        {
            Apply<Events.Seeded>(x => { });
        }
    }
}
