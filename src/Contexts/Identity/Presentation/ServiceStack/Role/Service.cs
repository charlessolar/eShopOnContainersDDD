using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Identity.Role
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task Any(Services.RoleActivate request)
        {
            return _bus.CommandToDomain(new Commands.Activate
            {
                RoleId=request.RoleId
            });
        }
        public Task Any(Services.RoleDeactivate request)
        {
            return _bus.CommandToDomain(new Commands.Deactivate
            {
                RoleId = request.RoleId
            });
        }
        public Task Any(Services.RoleDefine request)
        {
            return _bus.CommandToDomain(new Commands.Define
            {
                RoleId = request.RoleId,
                Name=request.Name
            });
        }
        public Task Any(Services.RoleDestroy request)
        {
            return _bus.CommandToDomain(new Commands.Destroy
            {
                RoleId = request.RoleId
            });
        }
        public Task Any(Services.RoleRevoke request)
        {
            return _bus.CommandToDomain(new Commands.Revoke
            {
                RoleId = request.RoleId
            });
        }
    }
}
