using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Catalog.CategoryBrand
{
    public class Handler :
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CategoryBrand
            {
                Id = e.BrandId,
                Brand = e.Brand
            };
            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.BrandId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.CategoryBrand>(e.BrandId);
        }
    }
}
