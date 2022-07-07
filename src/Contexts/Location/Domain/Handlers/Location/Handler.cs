using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Domain;
using NServiceBus;

namespace eShop.Location.Location
{
    public class Handler :
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.Remove>,
        IHandleMessages<Commands.UpdateDescription>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var location = await ctx.For<Location>().New(command.LocationId).ConfigureAwait(false);
            var parent = await ctx.For<Location>().TryGet(command.ParentId).ConfigureAwait(false);
            location.Add(command.Code, command.Description, parent?.State);
        }

        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var location = await ctx.For<Location>().Get(command.LocationId).ConfigureAwait(false);
            location.Remove();
        }

        public async Task Handle(Commands.UpdateDescription command, IMessageHandlerContext ctx)
        {
            var location = await ctx.For<Location>().Get(command.LocationId).ConfigureAwait(false);
            location.UpdateDescription(command.Description);
        }
    }
}
