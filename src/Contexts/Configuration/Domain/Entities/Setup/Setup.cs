using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup
{
    public class Setup : Aggregates.Entity<Setup, State>
    {
        private Setup() { }

        public void Seed()
        {
            Apply<Events.Seeded>(x => { });
        }
    }
}
