using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Application;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.CatalogBrand
{
    public class Handler :
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CatalogBrand
            {
                Id = e.BrandId,
                Brand = e.Brand
            };
            return ctx.Uow().Add(e.BrandId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.Uow().Delete<Models.CatalogBrand>(e.BrandId);
        }
    }
}
