using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/users/{UserId}/disable", "POST")]
    public class UserDisable : DomainCommand
    {
        public Guid UserId { get; set; }
    }
}
