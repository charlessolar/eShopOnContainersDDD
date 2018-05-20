using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using NServiceBus;

namespace eShop.Ordering
{
    [Category("Ordering")]
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

            await _bus.CommandToDomain(new Buyer.Commands.Initiate
            {
                GivenName = "administrator",
                UserName = "administrator",
            }).ConfigureAwait(false);

            await _bus.CommandToDomain(new Buyer.Entities.Address.Commands.Add
            {
                UserName = "administrator",
                AddressId = Guid.NewGuid(),
                Alias="Home",
                Street= "866 Arbutus Drive",
                City="Miami",
                State="FL",
                ZipCode = "33012",
                Country ="USA",
            }).ConfigureAwait(false);

            await _bus.CommandToDomain(new Buyer.Entities.PaymentMethod.Commands.Add
            {
                UserName = "administrator",
                PaymentMethodId = Guid.NewGuid(),
                Alias = "Mastercard",
                CardholderName = "C. Morgan",
                CardNumber= "5350 9832 5988 2498",
                CardType = Buyer.Entities.PaymentMethod.CardType.Mastercard,
                Expiration = DateTime.Now.AddMonths(6),
                SecurityNumber = "596",
            }).ConfigureAwait(false);

            this.Started = true;
            return true;
        }

        public bool Started { get; private set; }
    }
}
