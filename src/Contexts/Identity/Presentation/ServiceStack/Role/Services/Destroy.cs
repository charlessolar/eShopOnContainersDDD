using System;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.Role.Services
{
    [Api("Identity")]
    [Route("/identity/roles/{RoleId}", "DELETE")]
    public class RoleDestroy : DomainCommand
    {
        public Guid RoleId { get; set; }
    }
}
