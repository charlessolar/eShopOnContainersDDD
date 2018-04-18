using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Location.Location.Entities.Point.Models
{
    public class Point
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
