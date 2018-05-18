using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order
{
    public class State : Aggregates.State<State>
    {
        public string UserName { get; private set; }

        private void Handle(Events.Drafted e)
        {
            UserName = e.UserName;
        }
    }
}
