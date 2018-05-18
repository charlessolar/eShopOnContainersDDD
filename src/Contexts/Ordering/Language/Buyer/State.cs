using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer
{
    public class State : Aggregates.State<State>
    {
        public bool GoodStanding { get; private set; }

        private void Handle(Events.Initiated e)
        {
            this.GoodStanding = true;
        }

        private void Handle(Events.InGoodStanding e)
        {
            this.GoodStanding = true;
        }

        private void Handle(Events.Suspended e)
        {
            this.GoodStanding = false;
        }
    }
}
