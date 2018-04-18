using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Identity.User.Events
{
    public interface Registered : IStampedEvent
    {
        Guid UserId { get; set; }
        string GivenName { get; set; }
    }
}
