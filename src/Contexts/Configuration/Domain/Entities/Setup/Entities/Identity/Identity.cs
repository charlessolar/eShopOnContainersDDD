using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Identity
{
    public class Identity : Aggregates.Entity<Identity, State, Setup>
    {
        public void Seeded()
        {
            Apply<Events.Seeded>(x => { });
        }
    }
}
