using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Basket.Basket.Entities.Item.Commands
{
    public class UpdateQuantity : StampedCommand
    {
        public Guid BasketId { get; set; }
        public Guid ItemId { get; set; }
    
        public decimal Quantity { get; set; }
    }
}
