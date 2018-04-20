using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Location.Location.Entities.Point
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task Any(Services.AddPoint request)
        {
            return _bus.CommandToDomain(new Commands.Add
            {
                LocationId = request.LocationId,
                PointId = request.PointId,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            });
        }

        public Task Any(Services.RemovePoint request)
        {
            return _bus.CommandToDomain(new Commands.Remove
            {
                LocationId = request.LocationId,
                PointId = request.PointId
            });
        }
    }
}
