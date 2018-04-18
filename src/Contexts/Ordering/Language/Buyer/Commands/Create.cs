using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Commands
{
    public class Create : StampedCommand
    {
        public Guid BuyerId { get; set; }


    }
}
