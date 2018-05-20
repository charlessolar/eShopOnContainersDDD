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
                x.UserName = Parent.Id;
                x.PaymentMethodId = Id;
                x.Alias = alias;
                x.CardNumber = cardNumber;
                x.SecurityNumber = securityNumber;
                x.CardholderName = cardholderName;
                x.Expiration = expiration;
                x.CardType = type;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.UserName = Parent.Id;
                x.PaymentMethodId = Id;
            });
        }
    }
}
