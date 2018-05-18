using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Commands
{
    public class SetPreferredPaymentMethod : StampedCommand
    {
        public string UserName { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
