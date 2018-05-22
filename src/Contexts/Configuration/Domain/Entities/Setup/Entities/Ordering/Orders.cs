using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Ordering
{
    public class Orders : Aggregates.Entity<Orders, State, Setup>
    {
        private Orders() { }

        public void Seeded()
        {
            Apply<Events.Seeded>(x => { });
        }
    }
}
