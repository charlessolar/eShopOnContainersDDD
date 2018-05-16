using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;
using ServiceStack;

namespace eShop.Configuration.Setup
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }
        public Task<object> Any(Services.GetStatus request)
        {
            return _bus.RequestQuery<Queries.Status, Models.ConfigurationStatus>(new Queries.Status
            {
            });
        }

        public Task Any(Services.Seed request)
        {
            return _bus.CommandToDomain(new Commands.Seed
            {
            });
        }
    }
}
