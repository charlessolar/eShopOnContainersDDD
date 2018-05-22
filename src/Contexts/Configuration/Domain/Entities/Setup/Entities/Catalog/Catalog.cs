using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Entities.Catalog
{
    public class Catalog : Aggregates.Entity<Catalog, State, Setup>
    {
        private Catalog() { }

        public void Seeded()
        {
            Apply<Events.Seeded>(x => { });
        }
    }
}
