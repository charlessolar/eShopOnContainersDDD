using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Events
{
    public interface PasswordChanged : IStampedEvent
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
