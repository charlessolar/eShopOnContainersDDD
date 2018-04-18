using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Location.Location.Entities.Point
{
    public class Handler :
        IHandleMessages<Commands.Add>,
        IHandleMessages<Commands.Remove>
    {
        public async Task Handle(Commands.Add command, IMessageHandlerContext ctx)
        {
            var location = await ctx.For<Location>().Get(command.LocationId).ConfigureAwait(false);
            var point = await location.For<Point>().New(command.PointId).ConfigureAwait(false);
            point.Add(command.Longitude, command.Latitude);
        }

        public async Task Handle(Commands.Remove command, IMessageHandlerContext ctx)
        {
            var location = await ctx.For<Location>().Get(command.LocationId).ConfigureAwait(false);
            var point = await location.For<Point>().Get(command.PointId).ConfigureAwait(false);
            point.Remove();
        }
    }
}
