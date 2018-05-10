using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.Role
{
    public class State : Aggregates.State<State>
    {
        public bool Disabled { get; private set; }
        public bool Destroyed { get; private set; }

        private void Handle(Events.Activated e)
        {
            this.Disabled = false;
        }

        private void Handle(Events.Deactivated e)
        {
            this.Disabled = true;
        }

        private void Handle(Events.Destroyed e)
        {
            this.Destroyed = true;
        }
    }
}
