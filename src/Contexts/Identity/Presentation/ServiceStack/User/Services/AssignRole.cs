using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/users/{UserName}/assign", "POST")]
    public class AssignRole : DomainCommand
    {
        public string UserName { get; set; }
        public Guid RoleId { get; set; }
    }
}
