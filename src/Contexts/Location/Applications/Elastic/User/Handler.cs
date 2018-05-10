using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Location.User
{
    public class Handler :
        IHandleMessages<Events.Recorded>
    {
        public async Task Handle(Events.Recorded e, IMessageHandlerContext ctx)
        {
            var location = await ctx.App<Infrastructure.IUnitOfWork>().Get<Location.Models.Location>(e.LocationId)
                .ConfigureAwait(false);
            var user = await ctx.App<Infrastructure.IUnitOfWork>().Get<Identity.User.Models.User>(e.UserId)
                .ConfigureAwait(false);

            var model = new Models.Record
            {
                Id = e.RecordId,
                Code = location.Code,
                LocationId = location.Id,
                Name = user.GivenName,
                UserName = user.Id
            };

            await ctx.App<Infrastructure.IUnitOfWork>().Add(e.RecordId, model).ConfigureAwait(false);
        }
    }
}
