using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer.Entities.Address
{
    public class Handler :
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.Removed>
    {
        public Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var model = new Models.Address
            {
                Id = e.AddressId,
                UserName = e.UserName,
                Alias = e.Alias,
                City = e.City,
                State = e.State,
                Country = e.Country,
                Street = e.Street,
                ZipCode = e.ZipCode
            };

            return ctx.UoW().Add(e.AddressId, model);
        }

        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.Address>(e.AddressId);
        }
    }
}
