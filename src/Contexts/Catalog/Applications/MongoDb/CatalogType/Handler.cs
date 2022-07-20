using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Aggregates.Application;
using NServiceBus;

namespace eShop.Catalog.CatalogType
{
    public class Handler : 
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CatalogType
            {
                Id = e.TypeId,
                Type = e.Type
            };
            return ctx.Uow().Add(e.TypeId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.Uow().Delete<Models.CatalogType>(e.TypeId);
        }
    }
}
