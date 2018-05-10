using System;
using System.Collections.Generic;
using System.Text;
using Aggregates;

namespace eShop.Identity.Role
{
    public class Role : Aggregates.Entity<Role, State>
    {
        private Role() { }

        public void Activate()
        {
            if (State.Destroyed)
                throw new BusinessException("Role is already destroyed");
            if (!State.Disabled)
                throw new BusinessException("Role is not disabled");

            Apply<Events.Activated>(x => { x.RoleId = Id; });
        }

        public void Deactivate()
        {
            if (State.Destroyed)
                throw new BusinessException("Role is already destroyed");
            if (State.Disabled)
                throw new BusinessException("Role is already disabled");

            Apply<Events.Deactivated>(x => { x.RoleId = Id; });
        }

        public void Define(string name)
        {
            Apply<Events.Defined>(x =>
            {
                x.RoleId = Id;
                x.Name = name;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(x => { x.RoleId = Id; });
        }

        public void Revoke()
        {
            if (State.Destroyed)
                throw new BusinessException("Role is already destroyed");

            Apply<Events.Revoked>(x => { x.RoleId = Id; });
        }
    }
}
