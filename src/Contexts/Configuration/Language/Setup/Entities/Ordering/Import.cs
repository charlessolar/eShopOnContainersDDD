using Aggregates;
using Infrastructure.Extensions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using System.Linq;

namespace eShop.Configuration.Setup.Entities.Ordering
{
    public class Import
    {
        public static Types.Buyer[] Buyers = new Types.Buyer[] { };

        public static async Task Seed(IMessageHandlerContext ctx)
        {
            var addressFaker = new Faker<Types.Address>()
                .StrictMode(false)
                .Rules((f, o) =>
                {
                    o.AddressId = Guid.NewGuid();
                    o.Street = f.Address.StreetAddress();
                    o.City = f.Address.City();
                    o.State = f.Address.StateAbbr();
                    o.ZipCode = f.Address.ZipCode();
                    o.Country = "US";
                    o.Alias = o.City;
                });
            var paymentFaker = new Faker<Types.PaymentMethod>()
                .StrictMode(false)
                .Rules((f, o) =>
                {
                    o.PaymentMethodId = Guid.NewGuid();
                    o.CardholderName = f.Name.FindName(withPrefix: false, withSuffix: false);
                    o.CardNumber = f.Finance.CreditCardNumber();
                    o.Expiration = DateTime.UtcNow.AddMonths(2);
                    o.SecurityNumber = f.Finance.CreditCardCvv();
                    o.CardType = eShop.Ordering.Buyer.Entities.PaymentMethod.CardType.GetAll().RandomPick();
                    o.Alias = o.CardType.DisplayName;
                });

            // create buyers for all users
            Buyers = Identity.Import.Users.Select(user =>
            {
                var address = addressFaker.Generate();
                var method = paymentFaker.Generate();

                return new Types.Buyer
                {
                    UserName = user.UserName,
                    GivenName = user.GivenName,
                    Address = address,
                    PaymentMethod = method
                };
            }).ToArray();

            var saga = ctx.Saga(Guid.NewGuid());

            foreach (var buyer in Buyers)
            {
                saga.Command(new eShop.Ordering.Buyer.Commands.Initiate
                {
                    GivenName = buyer.GivenName,
                    UserName = buyer.UserName
                }).Command(new eShop.Ordering.Buyer.Entities.Address.Commands.Add
                {
                    AddressId = buyer.Address.AddressId,
                    Street = buyer.Address.Street,
                    City = buyer.Address.City,
                    State = buyer.Address.State,
                    ZipCode = buyer.Address.ZipCode,
                    Country = buyer.Address.Country,
                    Alias = buyer.Address.Alias,
                    UserName = buyer.UserName
                }).Command(new eShop.Ordering.Buyer.Entities.PaymentMethod.Commands.Add
                {
                    PaymentMethodId = buyer.PaymentMethod.PaymentMethodId,
                    CardholderName = buyer.PaymentMethod.CardholderName,
                    CardNumber = buyer.PaymentMethod.CardNumber,
                    CardType = buyer.PaymentMethod.CardType,
                    Expiration = buyer.PaymentMethod.Expiration,
                    SecurityNumber = buyer.PaymentMethod.SecurityNumber,
                    Alias = buyer.PaymentMethod.Alias,
                    UserName = buyer.UserName
                }).Command(new eShop.Ordering.Buyer.Commands.SetPreferredAddress
                {
                    UserName = buyer.UserName,
                    AddressId = buyer.Address.AddressId
                }).Command(new eShop.Ordering.Buyer.Commands.SetPreferredPaymentMethod
                {
                    UserName = buyer.UserName,
                    PaymentMethodId = buyer.PaymentMethod.PaymentMethodId
                });
            }
            await saga.Start().ConfigureAwait(false);


            var saga2 = ctx.Saga(Guid.NewGuid());
            var random = new Random();
            // create orders out of all baskets
            foreach (var basket in Basket.Import.Baskets)
            {
                var buyer = Buyers.Single(x => x.UserName == basket.UserName);

                var orderid = Guid.NewGuid();
                saga2.Command(new eShop.Ordering.Order.Commands.Draft
                {
                    BasketId = basket.Id,
                    OrderId = orderid,
                    UserName = basket.UserName,
                    BillingAddressId = buyer.Address.AddressId,
                    ShippingAddressId = buyer.Address.AddressId,
                    PaymentMethodId = buyer.PaymentMethod.PaymentMethodId,
                    Stamp = DateTime.UtcNow.RandomDateTimeBackward(TimeSpan.FromDays(1), TimeSpan.FromDays(14)).ToUnix()
                });

                // Use random below to vary the status of generated orders
                if (random.Next(3) == 0)
                {
                    saga2.Command(new eShop.Ordering.Order.Commands.Cancel
                    {
                        OrderId = orderid
                    });
                    continue;
                }

                if (random.Next(2) != 0)
                    continue;

                saga2.Command(new eShop.Ordering.Order.Commands.Confirm
                {
                    OrderId = orderid
                });

                if (random.Next(2) != 0)
                    continue;

                saga2.Command(new eShop.Ordering.Order.Commands.Pay
                {
                    OrderId = orderid
                });

                if (random.Next(2) != 0)
                    continue;

                saga2.Command(new eShop.Ordering.Order.Commands.Ship
                {
                    OrderId = orderid
                });
            }
            await saga2.Start().ConfigureAwait(false);
        }
    }
}
