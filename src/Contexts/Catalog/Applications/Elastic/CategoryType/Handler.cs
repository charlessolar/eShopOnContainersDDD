using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using NServiceBus;

namespace eShop.Catalog.CategoryType
{
    public class Handler : 
        IHandleMessages<Events.Defined>,
        IHandleMessages<Events.Destroyed>
    {
        public Task Handle(Events.Defined e, IMessageHandlerContext ctx)
        {
            var model = new Models.CategoryType
            {
                Id = e.TypeId,
                Type = e.Type
            };
            return ctx.App<Infrastructure.IUnitOfWork>().Add(e.TypeId, model);
        }
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.CategoryType>(e.TypeId);
        }
    }
}
