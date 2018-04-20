using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Identity.User
{
    public class Handler :
        IHandleMessages<Events.Registered>
    {
        public Task Handle(Events.Registered e, IMessageHandlerContext ctx)
        {
            var model = new Models.User
            {
                Id = e.UserId,
                GivenName = e.GivenName
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.UserId, model);
        }
    }
}
