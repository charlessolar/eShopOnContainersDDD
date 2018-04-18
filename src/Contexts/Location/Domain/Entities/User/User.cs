using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Location.User
{
    public class User : Aggregates.Entity<User, State>
    {
        private User() { }

        public void Record(Location.State location, Identity.User.State user)
        {
            Apply<Events.Recorded>(x =>
            {
                x.RecordId = Id;
                x.LocationId = location.Id;
                x.UserId = user.Id;
            });
        }
    }
}
