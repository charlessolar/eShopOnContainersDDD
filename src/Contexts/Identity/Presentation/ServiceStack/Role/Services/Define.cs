using System;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.Role.Services
{
    [Api("Identity")]
    [Route("/identity/roles", "POST")]
    public class RoleDefine : DomainCommand
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
    }
}
