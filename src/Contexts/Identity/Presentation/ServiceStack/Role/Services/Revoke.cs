using System;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.Role.Services
{
    [Api("Identity")]
    [Route("/identity/roles/{RoleId}/revoke", "POST")]
    public class RoleRevoke : DomainCommand
    {
        public Guid RoleId { get; set; }
    }
}
