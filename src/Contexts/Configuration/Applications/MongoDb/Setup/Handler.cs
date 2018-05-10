using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Configuration.Setup
{
    public class Handler :
        IHandleMessages<Events.Seeded>
    {
        public Task Handle(Events.Seeded e, IMessageHandlerContext ctx)
        {
            var model = new Models.Status
            {
                IsSetup = true
            };

            return ctx.App<Infrastructure.IUnitOfWork>().Add("setup", model);
        }
    }
}
