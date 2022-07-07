using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Aggregates.Domain;

namespace eShop.Location.User
{
    public class Handler :
        IHandleMessages<Commands.Record>
    {
        public async Task Handle(Commands.Record command, IMessageHandlerContext ctx)
        {
            var record = await ctx.For<User>().New(command.RecordId).ConfigureAwait(false);

            var location = await ctx.For<Location.Location>().Get(command.LocationId).ConfigureAwait(false);
            var user = await ctx.For<Identity.User.User>().Get(command.UserId).ConfigureAwait(false);
            record.Record(location.State, user.State);
        }
    }
}
