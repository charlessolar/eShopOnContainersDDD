using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup
{
    public class State : Aggregates.State<State>
    {
        public Boolean Seeded { get; private set; }

        private void Handle(Events.Seeded e)
        {
            this.Seeded = true;
        }
    }
}
