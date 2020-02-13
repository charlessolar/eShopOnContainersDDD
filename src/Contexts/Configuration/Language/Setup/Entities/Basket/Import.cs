using Aggregates;
using Infrastructure.Extensions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Basket
{
    public class Import
    {
        public static Types.Basket[] Baskets = new Types.Basket[] { };

        public static async Task Seed(IMessageHandlerContext ctx)
        {
            var random = new Random();
            // create 40 random baskets (which will be orders eventually)
            Baskets = Enumerable.Range(0, 100).Select(x => new Types.Basket
            {
                Id = Guid.NewGuid(),
                UserName = Identity.Import.Users.RandomPick().UserName,
                Products = Catalog.Import.Products.RandomPicks(3).Select(p => Tuple.Create(p.Id, (long)random.Next(3) + 1)).ToArray()
            }).ToArray();

            var saga = ctx.Saga(Guid.NewGuid());

            foreach (var basket in Baskets)
            {
                saga.Command(new eShop.Basket.Basket.Commands.Initiate
                {
                    BasketId = basket.Id,
                    UserName = basket.UserName
                });

                foreach (var product in basket.Products)
                {
                    saga.Command(new eShop.Basket.Basket.Entities.Item.Commands.AddItem
                    {
                        BasketId = basket.Id,
                        ProductId = product.Item1
                    });

                    saga.Command(new eShop.Basket.Basket.Entities.Item.Commands.UpdateQuantity
                    {
                        BasketId = basket.Id,
                        ProductId = product.Item1,
                        Quantity = product.Item2
                    });
                }
            }

            await saga.Start().ConfigureAwait(false);
        }
    }
}
