using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Ordering.Buyer.Commands
{
    public class MarkGoodStanding : StampedCommand
    {
        public string UserName { get; set; }
    }
}
