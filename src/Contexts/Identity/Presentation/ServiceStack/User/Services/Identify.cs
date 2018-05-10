using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/identify", "POST")]
    public class Identify : DomainCommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
