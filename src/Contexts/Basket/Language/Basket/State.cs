using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Basket.Basket
{
    public class State : Aggregates.State<State>
    {
        public string UserName { get; private set; }

        public void Handle(Events.Initiated e)
        {
            this.UserName = e.UserName;
        }

        public void Handle(Events.BasketClaimed e)
        {
            this.UserName = e.UserName;
        }
    }
}
