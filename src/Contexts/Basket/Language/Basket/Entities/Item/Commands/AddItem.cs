using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Basket.Basket.Entities.Item.Commands
{
    public class AddItem : StampedCommand
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
    }
}
