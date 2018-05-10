using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/users/{UserId}/enable", "POST")]
    public class UserEnable : DomainCommand
    {
        public Guid UserId { get; set; }
    }
}
