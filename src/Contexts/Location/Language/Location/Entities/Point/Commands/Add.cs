using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Entities.Point.Commands
{
    public class Add : StampedCommand
    {
        public Guid LocationId { get; set; }
        public Guid PointId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
