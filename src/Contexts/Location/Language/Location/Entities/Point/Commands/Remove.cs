using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Entities.Point.Commands
{
    public class Remove : StampedCommand
    {
        public Guid LocationId { get; set; }
        public Guid PointId { get; set; }
    }
}
