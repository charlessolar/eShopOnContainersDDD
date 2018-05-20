using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Commands
{
    public class Draft : StampedCommand
    {
        public Guid OrderId { get; set; }

        public string UserName { get; set; }
        public Guid BasketId { get; set; }

        public Guid ShippingAddressId { get; set; }
        public Guid BillingAddressId { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
