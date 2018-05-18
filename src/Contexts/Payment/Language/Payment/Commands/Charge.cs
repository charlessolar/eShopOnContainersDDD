using Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment.Commands
{
    public class Charge : StampedCommand
    {
        public Guid PaymentId { get; set; }

        public string UserName { get; set; }

        public Guid OrderId { get; set; }
        public Guid PaymentMethodId { get; set; }
    }
}
