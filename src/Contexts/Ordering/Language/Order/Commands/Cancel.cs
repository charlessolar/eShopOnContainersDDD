using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Commands
{
    public class Cancel : StampedCommand
    {
        public Guid OrderId { get; set; }
    }
}
