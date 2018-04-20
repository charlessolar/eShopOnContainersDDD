using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Location.Location
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.ListLocations request)
        {
            return _bus.RequestPaged<Queries.Locations, Models.Location>(new Queries.Locations
            {
            });
        }

        public Task Any(Services.AddLocation request)
        {
            return _bus.CommandToDomain(new Commands.Add
            {
                LocationId = request.LocationId,
                Code = request.Code,
                Description = request.Description,
                ParentId = request.ParentId
            });
        }

        public Task Any(Services.RemoveLocation request)
        {
            return _bus.CommandToDomain(new Commands.Remove
            {
                LocationId = request.LocationId
            });
        }

        public Task Any(Services.UpdateDescriptionLocation request)
        {
            return _bus.CommandToDomain(new Commands.UpdateDescription
            {
                LocationId = request.LocationId,
                Description = request.Description
            });
        }
    }
}
