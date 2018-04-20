using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Identity.User.Services
{
    [Api("Identity")]
    [Route("/identity/register", "POST")]
    public class UserRegister : DomainCommand
    {
        public Guid UserId { get; set; }
        public string GivenName { get; set; }
    }
}
