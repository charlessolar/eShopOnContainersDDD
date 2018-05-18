using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Payment.Payment.Commands
{
    public class Settle : StampedCommand
    {
        public Guid PaymentId { get; set; }
    }
}
