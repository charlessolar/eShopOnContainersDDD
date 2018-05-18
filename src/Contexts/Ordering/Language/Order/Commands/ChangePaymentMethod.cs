using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Commands
{
    public class ChangePaymentMethod : StampedCommand
    {
        public Guid OrderId { get; set; }

        public Guid PaymentMethodId { get; set; }
    }
}
