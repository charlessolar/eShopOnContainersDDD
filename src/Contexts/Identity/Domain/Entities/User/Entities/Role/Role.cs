using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Entities.Role
{
    public class Role : Aggregates.Entity<Role, State, User>
    {
        private Role() { }

        public void Assign()
        {
            Apply<Events.Assigned>(x =>
            {
                x.UserId = Parent.Id;
                x.RoleId = Id;
            });
        }

        public void Revoke()
        {
            Apply<Events.Revoked>(x =>
            {
                x.UserId = Parent.Id;
                x.RoleId = Id;
            });
        }
    }
}
