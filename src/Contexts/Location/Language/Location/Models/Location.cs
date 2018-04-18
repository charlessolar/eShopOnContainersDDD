using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.Location.Models
{
    public class Location : StampedCommand
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public Entities.Point.Models.Point[] Points { get; set; }
    }
}
