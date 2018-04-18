using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Commands
{
    public class Draft : StampedCommand
    {
        public Guid OrderId { get; set; }

        public Guid BuyerId { get; set; }
        public Guid CartId { get; set; }
    }
}
