using System;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.Role.Services
{
    [Api("Identity")]
    [Route("/identity/roles/{RoleId}/activate", "POST")]
    public class RoleActivate : DomainCommand
    {
        public Guid RoleId { get; set; }
    }
}
