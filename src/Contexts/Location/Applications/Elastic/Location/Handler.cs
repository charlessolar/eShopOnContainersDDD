using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Location.Location
{
    public class Handler :
        IHandleMessages<Events.Added>,
        IHandleMessages<Events.Removed>
    {
        public Task Handle(Events.Added e, IMessageHandlerContext ctx)
        {
            var model = new Models.Location
            {
                Id = e.LocationId,
                Code = e.Code,
                Description = e.Description,
                Stamp = e.Stamp
            };
            return ctx.UoW().Add(e.LocationId, model);
        }
        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.Location>(e.LocationId);
        }

    }
}
