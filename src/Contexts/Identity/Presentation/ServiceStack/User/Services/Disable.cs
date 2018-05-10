using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/users/{UserName}/disable", "POST")]
    public class UserDisable : DomainCommand
    {
        public string UserName { get; set; }
    }
}
