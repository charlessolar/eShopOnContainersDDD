using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Marketing.Campaign.Commands
{
    public class Define : StampedCommand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
