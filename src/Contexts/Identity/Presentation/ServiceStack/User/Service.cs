using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Identity.User
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task Any(Services.UserRegister request)
        {
            return _bus.CommandToDomain(new Commands.Register
            {
                UserId = request.UserId,
                GivenName = request.GivenName
            });
        }
    }
}
