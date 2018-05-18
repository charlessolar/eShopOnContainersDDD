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

        public Task<object> Any(Services.GetIdentity request)
        {
            var session = GetSession();
            if (!session.IsAuthenticated)
                throw new HttpError("not logged in");
            
            return _bus.RequestQuery<Queries.Identity, Models.User>(new Queries.Identity
            {
                UserName = session.UserName
            });
        }

        public Task<object> Any(Services.GetUsers request)
        {
            return _bus.RequestPaged<Queries.Users, Models.User>(new Queries.Users { });
        }

        public Task Any(Services.UserRegister request)
        {
            return _bus.CommandToDomain(new Commands.Register
            {
                UserName = request.UserName,
                GivenName = request.GivenName
            });
        }

        public Task Any(Services.ChangeName request)
        {
            return _bus.CommandToDomain(new Commands.ChangeName
            {
                UserName = request.UserName,
                GivenName = request.GivenName
            });
        }
        public Task Any(Services.ChangePassword request)
        {
            return _bus.CommandToDomain(new Commands.ChangePassword
            {
                UserName = request.UserName,
                Password = request.Password,
                NewPassword = request.NewPassword
            });
        }
        public Task Any(Services.UserEnable request)
        {
            return _bus.CommandToDomain(new Commands.Enable
            {
                UserName = request.UserName,
            });
        }
        public Task Any(Services.UserDisable request)
        {
            return _bus.CommandToDomain(new Commands.Disable
            {
                UserName = request.UserName,
            });
        }

        public Task Any(Services.AssignRole request)
        {
            return _bus.CommandToDomain(new Entities.Role.Commands.Assign
            {
                UserName = request.UserName,
                RoleId = request.RoleId
            });
        }
        public Task Any(Services.RevokeRole request)
        {
            return _bus.CommandToDomain(new Entities.Role.Commands.Revoke
            {
                UserName = request.UserName,
                RoleId = request.RoleId
            });
        }
    }
}
