using Infrastructure.Extensions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Ordering
{
    public class Import
    {
        public static Task Seed(IMessageHandlerContext ctx)
        {
            return Task.CompletedTask;
        }
    }
}
