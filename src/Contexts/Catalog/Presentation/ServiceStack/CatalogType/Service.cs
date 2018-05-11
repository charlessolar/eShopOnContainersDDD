using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Catalog.CatalogType
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.ListCatalogTypes request)
        {
            return _bus.RequestPaged<Queries.Types, Models.CatalogType>(new Queries.Types
            {
                Term = request.Term,
                Limit = request.Limit
            });
        }

        public Task Any(Services.AddCatalogType request)
        {
            return _bus.CommandToDomain(new Commands.Define
            {
                TypeId = request.TypeId,
                Type = request.Type
            });
        }
        public Task Any(Services.RemoveCatalogType request)
        {
            return _bus.CommandToDomain(new Commands.Destroy
            {
                TypeId = request.TypeId
            });
        }
    }
}
