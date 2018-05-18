using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment
{
    public class Payment : Aggregates.Entity<Payment, State>
    {
        private Payment() { }

        public void Charge(Ordering.Buyer.State buyer, Ordering.Order.State order, Ordering.Buyer.Entities.PaymentMethod.State method)
        {
            Apply<Events.Charged>(x =>
            {
                x.PaymentId = Id;
                x.UserName = buyer.Id;
                x.OrderId = order.Id;
                x.PaymentMethodId = method.Id;
            });
        }
        public void Settle()
        {
            Apply<Events.Settled>(x =>
            {
                x.PaymentId = Id;
            });
        }
        public void Cancel()
        {
            Apply<Events.Canceled>(x =>
            {
                x.PaymentId = Id;
            });
        }
    }
}
