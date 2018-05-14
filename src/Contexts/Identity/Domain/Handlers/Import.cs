using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using NServiceBus;

namespace eShop.Identity
{
    [Category("Identity")]
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
                UserName = "administrator",
                Password = "12345678"
            }).ConfigureAwait(false);

            await _bus.CommandToDomain(new Role.Commands.Define
            {
                RoleId = roleId,
                Name = "administrator"
            }).ConfigureAwait(false);

            await _bus.CommandToDomain(new User.Entities.Role.Commands.Assign
            {
                UserName = "administrator",
                RoleId = roleId
            }).ConfigureAwait(false);

            this.Started = true;
            return true;
        }

        public bool Started { get; private set; }
    }
}
