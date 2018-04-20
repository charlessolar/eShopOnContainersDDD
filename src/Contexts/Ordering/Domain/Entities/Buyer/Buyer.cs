using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer
{
    public class Buyer : Aggregates.Entity<Buyer, State>
    {
        private Buyer() { }

        public void Create(string givenName)
        {
            Apply<Events.Created>(x =>
            {
                x.BuyerId = Id;
                x.GivenName = givenName;
            });
        }
    }
}
