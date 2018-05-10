using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Events
{
    public interface Registered : IStampedEvent
    {
        string GivenName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}
