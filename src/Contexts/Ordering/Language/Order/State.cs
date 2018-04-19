using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order
{
    public class State : Aggregates.State<State>
    {
        public Guid BuyerId { get; private set; }

        private void Handle(Events.Drafted e)
        {
            BuyerId = e.BuyerId;
        }
    }
}
