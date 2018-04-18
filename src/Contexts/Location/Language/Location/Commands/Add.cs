using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Commands
{
    public class Add : StampedCommand
    {
        public Guid LocationId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Guid ParentId { get; set; }
    }
}
