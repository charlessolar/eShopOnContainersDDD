using System;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.Role.Services
{
    [Api("Identity")]
    [Route("/identity/roles/{RoleId}/deactivate", "POST")]
    public class RoleDeactivate : DomainCommand
    {
        public Guid RoleId { get; set; }
    }
}
