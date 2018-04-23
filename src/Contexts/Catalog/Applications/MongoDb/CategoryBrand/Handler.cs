using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;

namespace eShop.Catalog.CategoryBrand
{
    public class Handler :
        IHandleQueries<Queries.Brands>,
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public Task Handle(Queries.Brands q, IMessageHandlerContext ctx)
        {
            var uow = ctx.App<Infrastructure.IUnitOfWork>();

            return ctx.Result(new[]
            {
                new Models.CategoryBrand {Id = Guid.NewGuid(), Brand = "Test"}
            }, 1, 0);
        }

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
