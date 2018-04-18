using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Basket.Basket.Commands
{
    public class Destroy : StampedCommand
    {
        public Guid BasketId { get; set; }
    }
}
