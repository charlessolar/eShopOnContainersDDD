using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Location.Location
{
    public class Location : Aggregates.Entity<Location, State>
    {
        private Location() { }

        public void Add(string code, string description, State parent)
        {
            Apply<Events.Added>(x =>
            {
                x.LocationId = Id;
                x.Code = code;
                x.Description = description;
                x.ParentId = parent?.Id;
            });
        }

        public void UpdateDescription(string description)
        {
            Apply<Events.DescriptionUpdated>(x =>
            {
                x.LocationId = Id;
                x.Description = description;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.LocationId = Id;
            });
        }
    }
}
