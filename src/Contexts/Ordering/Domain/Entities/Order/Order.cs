using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order
{
    public class Order : Aggregates.Entity<Order, State>
    {
        private Order() { }

        public void Draft(Buyer.State buyer, Basket.Basket.State basket)
        {
            Apply<Events.Drafted>(x =>
            {
                x.OrderId = Id;
                x.BuyerId = buyer.Id;
                x.CartId = basket.Id;
            });

            // foreach item in card - add item?
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
        public void SetAddress(Buyer.Entities.Address.State address)
        {
            Apply<Events.AddressSet>(x =>
            {
                x.OrderId = Id;
                x.AddressId = address.Id;
            });
        }

        public void SetPaymentMethod(Buyer.Entities.PaymentMethod.State paymentMethod)
        {
            Apply<Events.PaymentMethodSet>(x =>
            {
                x.OrderId = Id;
                x.PaymentMethodId = paymentMethod.Id;
            });
        }

    }
}
