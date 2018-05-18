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
                City = e.City,
                State = e.State,
                Country = e.Country,
                Street = e.Street,
                ZipCode = e.ZipCode
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.AddressId, model);
        }

        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.Address>(e.AddressId);
        }
    }
}
