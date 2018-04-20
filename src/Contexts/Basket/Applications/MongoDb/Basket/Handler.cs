using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure;
using NServiceBus;

namespace eShop.Basket.Basket
{
    public class Handler : 
        IHandleMessages<Events.Destroyed>
    {
        public Task Handle(Events.Destroyed e, IMessageHandlerContext ctx)
        {
            return ctx.App<Infrastructure.IUnitOfWork>().Delete<Models.Basket>(e.BasketId);
        }
    }
}
