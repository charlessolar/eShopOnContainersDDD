using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/users/{UserId}/revoke", "POST")]
    public class RevokeRole : DomainCommand
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
