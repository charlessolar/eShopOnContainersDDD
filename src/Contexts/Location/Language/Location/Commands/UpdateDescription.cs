using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Commands
{
    public class UpdateDescription : StampedCommand
    {
        public Guid LocationId { get; set; }
        public string Description { get; set; }
    }
}
