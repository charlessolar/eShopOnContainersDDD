using System;
using System.Collections.Generic;
using System.Text;
using Aggregates;
using Infrastructure.Security;

namespace eShop.Identity.User
{
    public class User : Aggregates.Entity<User, State>
    {
        private User() { }

        public void Register(string givenName, string password)
        {
            var hashed = PasswordStorage.CreateHash(password);
            Apply<Events.Registered>(x =>
            {
                x.UserName = Id;
                x.GivenName = givenName;
                x.Password = hashed;
            });
        }

        public void Identify(string password)
        {
            if (State.Disabled)
                throw new BusinessException("user disabled");
            if (!PasswordStorage.VerifyPassword(password, State.HashedPassword))
                throw new BusinessException("invalid password");

            Apply<Events.Identified>(x => { x.UserName = Id; });
        }

        public void Disable()
        {
            if (State.Disabled)
                throw new BusinessException("user already disabled");
            Apply<Events.Disabled>(x => { x.UserName = Id; });
        }

        public void Enable()
        {
            if (!State.Disabled)
                throw new BusinessException("user already enabled");
            Apply<Events.Enabled>(x => { x.UserName = Id; });
        }

        public void ChangeName(string givenName)
        {
            Apply<Events.NameChanged>(x =>
            {
                x.UserName = Id;
                x.GivenName = givenName;
            });
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            if (!PasswordStorage.VerifyPassword(currentPassword, State.HashedPassword))
                throw new BusinessException("invalid current password");

            var hashed = PasswordStorage.CreateHash(newPassword);
            Apply<Events.PasswordChanged>(x =>
            {
                x.UserName = Id;
                x.Password = hashed;
            });
        }
    }
}
