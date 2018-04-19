using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Entities.PaymentMethod
{
    public class PaymentMethod : Aggregates.Entity<PaymentMethod, State, Buyer>
    {
        private PaymentMethod() { }

        public void Add(string alias, string cardNumber, string securityNumber, string cardholderName,
            DateTime expiration, CardType type)
        {
            Apply<Events.Added>(x =>
            {
                x.BuyerId = Parent.Id;
                x.PaymentMethodId = Id;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.BuyerId = Parent.Id;
                x.PaymentMethodId = Id;
            });
        }
    }
}
