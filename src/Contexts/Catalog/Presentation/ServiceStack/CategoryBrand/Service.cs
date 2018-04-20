using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using NServiceBus;

namespace eShop.Catalog.CategoryBrand
{
    public class Service : ServiceStack.Service
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }

        public Task<object> Any(Services.ListCategoryBrands request)
        {
            return _bus.RequestPaged<Queries.Brands, Models.CategoryBrand>(new Queries.Brands
            {
                Term = request.Term,
                Limit = request.Limit
            });
        }

        public Task Any(Services.AddCategoryBrand request)
        {
            return _bus.CommandToDomain(new Commands.Define
            {
                BrandId = request.BrandId,
                Brand = request.Brand
            });
        }
        public Task Any(Services.RemoveCategoryBrand request)
        {
            return _bus.CommandToDomain(new Commands.Destroy
            {
                BrandId = request.BrandId
            });
        }
    }
}
