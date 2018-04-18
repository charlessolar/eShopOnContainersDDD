using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Order.Entities.Item.Commands
{
    public class ChangeQuantity : StampedCommand
    {
        public Guid ItemId { get; set; }
        public Guid OrderId { get; set; }

        public decimal Quantity { get; set; }
    }
}
