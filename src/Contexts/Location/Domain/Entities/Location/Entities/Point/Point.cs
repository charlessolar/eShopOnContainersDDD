using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Location.Location.Entities.Point
{
    public class Point : Aggregates.Entity<Point, State, Location>
    {
        private Point() { }

        public void Add(double longtitude, double latitude)
        {
            Apply<Events.Added>(x =>
            {
                x.LocationId = Parent.Id;
                x.PointId = Id;
                x.Longitude = longtitude;
                x.Latitude = latitude;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.LocationId = Parent.Id;
                x.PointId = Id;
            });
        }
    }
}
