using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Identity.User
{
    public class State : Aggregates.State<State>
    {
        public string HashedPassword { get; private set; }
        public bool Disabled { get; private set; }

        private void Handle(Events.Disabled e)
        {
            this.Disabled = true;
        }

        private void Handle(Events.Enabled e)
        {
            this.Disabled = false;
        }

        private void Handle(Events.Registered e)
        {
            this.HashedPassword = e.Password;
        }
        private void Handle(Events.PasswordChanged e)
        {
            this.HashedPassword = e.Password;
        }
    }
}
