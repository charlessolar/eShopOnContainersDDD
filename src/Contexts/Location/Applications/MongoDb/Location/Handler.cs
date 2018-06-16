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
        IHandleMessages<Events.DescriptionUpdated>,
        IHandleMessages<Events.Removed>,
        IHandleMessages<Entities.Point.Events.Added>,
        IHandleMessages<Entities.Point.Events.Removed>
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
        public async Task Handle(Events.DescriptionUpdated e, IMessageHandlerContext ctx)
        {
            var location = await ctx.UoW().Get<Models.Location>(e.LocationId)
                .ConfigureAwait(false);

            location.Description = e.Description;

            await ctx.UoW().Update(e.LocationId, location).ConfigureAwait(false);
        }
        public Task Handle(Events.Removed e, IMessageHandlerContext ctx)
        {
            return ctx.UoW().Delete<Models.Location>(e.LocationId);
        }

        public async Task Handle(Entities.Point.Events.Added e, IMessageHandlerContext ctx)
        {
            var location = await ctx.UoW().Get<Models.Location>(e.LocationId)
                .ConfigureAwait(false);

            location.Points = location.Points.Concat(new[]
            {
                new Entities.Point.Models.Point
                {
                    Id = e.PointId,
                    LocationId = e.LocationId,
                    Latitude = e.Latitude,
                    Longitude = e.Longitude
                }
            }).ToArray();

            await ctx.UoW().Update(e.LocationId, location).ConfigureAwait(false);
        }
        public async Task Handle(Entities.Point.Events.Removed e, IMessageHandlerContext ctx)
        {
            var location = await ctx.UoW().Get<Models.Location>(e.LocationId)
                .ConfigureAwait(false);

            location.Points = location.Points.Where(x => x.Id != e.PointId).ToArray();

            await ctx.UoW().Update(e.LocationId, location).ConfigureAwait(false);
        }
    }
}
