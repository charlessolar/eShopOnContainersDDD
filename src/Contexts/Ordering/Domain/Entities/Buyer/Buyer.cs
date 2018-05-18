using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer
{
    public class Buyer : Aggregates.Entity<Buyer, State>
    {
        private Buyer() { }

        public void Initiate(string givenName)
        {
            Apply<Events.Initiated>(x =>
            {
                x.UserName = Id;
                x.GivenName = givenName;
            });
        }

        public void MarkGoodStanding()
        {
            Apply<Events.InGoodStanding>(x => { x.UserName = Id; });
        }

        public void MarkSuspended()
        {
            Apply<Events.Suspended>(x => { x.UserName = Id; });
        }

        public void SetPreferredAddress(Entities.Address.State address)
        {
            Apply<Events.PreferredAddressSet>(x =>
            {
                x.UserName = Id;
                x.AddressId = address.Id;
            });
        }

        public void SetPreferredPaymentMethod(Entities.PaymentMethod.State method)
        {
            Apply<Events.PreferredPaymentSet>(x =>
            {
                x.UserName = Id;
                x.PaymentMethodId = method.Id;
            });
        }
    }
}
