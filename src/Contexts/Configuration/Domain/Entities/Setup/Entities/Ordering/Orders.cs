using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Ordering
{
    public class Orders : Aggregates.Entity<Orders, State, Setup>
    {
        public void Seeded()
        {
            Apply<Events.Seeded>(x => { });
        }
    }
}
