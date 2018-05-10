using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Setup;
using NServiceBus;

namespace eShop.Identity
{
    public class Import : ISeed
    {
        private readonly IMessageSession _bus;

        public Import(IMessageSession bus)
        {
            _bus = bus;
        }

        public async Task<bool> Seed()
        {
            var roleId = Guid.NewGuid();

            await _bus.CommandToDomain(new User.Commands.Register
            {
                GivenName = "administrator",
                UserName = "admin",
                Password = "admin"
            }).ConfigureAwait(false);

            await _bus.CommandToDomain(new Role.Commands.Define
            {
                RoleId = roleId,
                Name = "admininstrator"
            }).ConfigureAwait(false);

            await _bus.CommandToDomain(new User.Entities.Role.Commands.Assign
            {
                UserName = "admin",
                RoleId = roleId
            }).ConfigureAwait(false);

            this.Started = true;
            return true;
        }

        public bool Started { get; private set; }
    }
}
