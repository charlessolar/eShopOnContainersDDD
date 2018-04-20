using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Catalog.CategoryType
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.ListCategoryTypes request)
        {
            return _bus.RequestPaged<Queries.Types, Models.CategoryType>(new Queries.Types
            {
                Term = request.Term,
                Limit = request.Limit
            });
        }

        public Task Any(Services.AddCategoryType request)
        {
            return _bus.CommandToDomain(new Commands.Define
            {
                TypeId = request.TypeId,
                Type = request.Type
            });
        }
        public Task Any(Services.RemoveCategoryType request)
        {
            return _bus.CommandToDomain(new Commands.Destroy
            {
                TypeId = request.TypeId
            });
        }
    }
}
