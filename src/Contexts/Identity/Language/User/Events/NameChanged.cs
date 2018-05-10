using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Events
{
    public interface NameChanged : IStampedEvent
    {
        string UserName { get; set; }
        string GivenName { get; set; }
    }
}
