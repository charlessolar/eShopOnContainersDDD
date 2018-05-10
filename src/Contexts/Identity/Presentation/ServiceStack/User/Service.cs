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
        public Task Any(Services.UserEnable request)
        {
            return _bus.CommandToDomain(new Commands.Enable
            {
                UserId = request.UserId,
            });
        }
        public Task Any(Services.UserDisable request)
        {
            return _bus.CommandToDomain(new Commands.Disable
            {
                UserId = request.UserId,
            });
        }

        public Task Any(Services.AssignRole request)
        {
            return _bus.CommandToDomain(new Entities.Role.Commands.Assign
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            });
        }
        public Task Any(Services.RevokeRole request)
        {
            return _bus.CommandToDomain(new Entities.Role.Commands.Revoke
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            });
        }
    }
}
