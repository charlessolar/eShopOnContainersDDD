
using System;
using System.Collections.Generic;
using System.Linq;
using Aggregates.Extensions;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using Serilog;
using ServiceStack;
using ServiceStack.Auth;

namespace eShop.Presentation.Authentication
{
    public class IdentitySessionSource : IUserSessionSource
    {
        private IMessageSession _bus;

        public IdentitySessionSource(IMessageSession bus)
        {
            _bus = bus;
        }
        public IAuthSession GetUserSession(string userAuthId)
        {
            var user = _bus.RequestQuery<Identity.User.Queries.Identity, Identity.User.Models.User>(
                new Identity.User.Queries.Identity
                {
                    UserName = userAuthId
                }).Result as Identity.User.Models.User;

            return new AuthUserSession()
            {
                UserName = user.Id,
                DisplayName = user.GivenName,
                Roles = user.Roles.ToList(),
            };
        }
    }

}
