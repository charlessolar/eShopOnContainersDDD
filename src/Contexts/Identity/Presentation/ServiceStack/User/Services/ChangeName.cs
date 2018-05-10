using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/users/{UserName}/name", "POST")]
    public class ChangeName : DomainCommand
    {
        public string UserName { get; set; }
        public string GivenName { get; set; }
    }
}
