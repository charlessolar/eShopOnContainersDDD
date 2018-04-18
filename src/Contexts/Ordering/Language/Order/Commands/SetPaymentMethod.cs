using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Commands
{
    public class SetPaymentMethod : StampedCommand
    {
        public Guid OrderId { get; set; }

        public Guid PaymentMethodId { get; set; }
    }
}
