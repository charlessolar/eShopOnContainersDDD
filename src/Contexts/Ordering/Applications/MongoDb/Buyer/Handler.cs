using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Ordering.Buyer
{
    public class Handler : 
        IHandleMessages<Events.Created>
    {
        public Task Handle(Events.Created e, IMessageHandlerContext ctx)
        {
            var model = new Models.Buyer
            {
                Id = e.BuyerId,
                GivenName = e.GivenName
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.BuyerId, model);
        }
    }
}
