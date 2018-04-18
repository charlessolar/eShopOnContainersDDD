using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Entities.Item.Commands
{
    public class Remove : StampedCommand
    {
        public Guid ItemId { get; set; }
        public Guid OrderId { get; set; }
    }
}
