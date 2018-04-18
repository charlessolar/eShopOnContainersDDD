using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.User
{
    public class User : Aggregates.Entity<User, State>
    {
        private User() { }

        public void Register(string givenName)
        {
            Apply<Events.Registered>(x =>
            {
                x.UserId = Id;
                x.GivenName = givenName;
            });
        }
    }
}
