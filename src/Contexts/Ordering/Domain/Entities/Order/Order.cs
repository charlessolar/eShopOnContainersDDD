using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order
{
    public class Order : Aggregates.Entity<Order, State>
    {
        private Order() { }

        public void Draft(Buyer.State buyer, Basket.Basket.State basket, Buyer.Entities.Address.State shipping, Buyer.Entities.Address.State billing, Buyer.Entities.PaymentMethod.State method)
        {
            Apply<Events.Drafted>(x =>
            {
                x.OrderId = Id;
                x.UserName = buyer.Id;
                x.BasketId = basket.Id;
                x.ShippingAddressId = shipping.Id;
                x.BillingAddressId = billing.Id;
                x.PaymentMethodId = method.Id;
            });

        }

        public void Cancel()
        {
            Apply<Events.Canceled>(x =>
            {
                x.OrderId = Id;
            });
        }

        public void Confirm()
        {
            Apply<Events.Confirm>(x =>
            {
                x.OrderId = Id;
            });
        }

        public void Pay()
        {
            Apply<Events.Paid>(x =>
            {
                x.OrderId = Id;
            });
        }

        public void Ship()
        {
            Apply<Events.Shipped>(x =>
            {
                x.OrderId = Id;
            });
        }
        public void ChangeAddress(Buyer.Entities.Address.State shipping, Buyer.Entities.Address.State billing)
        {
            Apply<Events.AddressChanged>(x =>
            {
                x.OrderId = Id;
                x.ShippingId = shipping.Id;
                x.BillingId = billing.Id;
            });
        }

        public void ChangePaymentMethod(Buyer.Entities.PaymentMethod.State paymentMethod)
        {
            Apply<Events.PaymentMethodChanged>(x =>
            {
                x.OrderId = Id;
                x.PaymentMethodId = paymentMethod.Id;
            });
        }

    }
}
