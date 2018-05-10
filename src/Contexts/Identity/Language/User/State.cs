using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.User
{
    public class State : Aggregates.State<State>
    {
        public bool Disabled { get; private set; }

        private void Handle(Events.Disabled e)
        {
            this.Disabled = true;
        }

        private void Handle(Events.Enabled e)
        {
            this.Disabled = false;
        }
    }
}
