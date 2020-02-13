using Aggregates;
using Infrastructure.Extensions;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Configuration.Setup.Entities.Catalog
{
    public class Import
    {
        public static readonly Types.CatalogBrand[] Brands = new[]
        {
            new Types.CatalogBrand {Id = Guid.NewGuid(), Brand = "Azure"},
            new Types.CatalogBrand {Id = Guid.NewGuid(), Brand = ".NET"},
            new Types.CatalogBrand {Id = Guid.NewGuid(), Brand = "Visual Studio"},
            new Types.CatalogBrand {Id = Guid.NewGuid(), Brand = "SQL Server"},
            new Types.CatalogBrand {Id = Guid.NewGuid(), Brand = "Other"},
        };

        public static readonly Types.CatalogType[] Types = new[]
        {
            new Types.CatalogType {Id = Guid.NewGuid(), Type = "Mug"},
            new Types.CatalogType {Id = Guid.NewGuid(), Type = "T-Shirt"},
            new Types.CatalogType {Id = Guid.NewGuid(), Type = "Sheet"},
            new Types.CatalogType {Id = Guid.NewGuid(), Type = "USB Memory Stick"},
        };
        public static readonly Types.Product[] Products = new[]
        {
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[1].Id, Name=".NET Bot Black Hoodie", Description = ".NET Bot Black Hoodie, and more", Picture="1.png", Price=1950, AvailableStock=100, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[0].Id, CatalogBrandId = Brands[1].Id, Name=".NET Black & White Mug", Description = ".NET Black & White Mug", Picture="2.png", Price=850, AvailableStock=90,RestockThreshold=10, OnReorder=true },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Prism White T-Shirt", Description = "Prism White T-Shirt", Picture="3.png", Price=1200, AvailableStock=55, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[1].Id, Name=".NET Foundation T-shirt", Description = ".NET Foundation T-shirt", Picture="4.png", Price=1200, AvailableStock=120, RestockThreshold=20, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[2].Id, CatalogBrandId = Brands[4].Id, Name="Roslyn Red Sheet", Description = "Roslyn Red Sheet", Picture="5.png", Price=850, AvailableStock=55,RestockThreshold=25, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[1].Id, Name=".NET Blue Hoodie", Description = ".NET Blue Hoodie", Picture="6.png", Price=1200, AvailableStock=20, RestockThreshold=10, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Roslyn Red T-Shirt", Description = "Roslyn Red T-Shirt", Picture="7.png", Price=1200, AvailableStock=10,RestockThreshold=2, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Kudu Purple Hoodie", Description = "Kudu Purple Hoodie", Picture="8.png", Price=850, AvailableStock=35, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[0].Id, CatalogBrandId = Brands[4].Id, Name="Cup<T> White Mug", Description = "Cup<T> White Mug", Picture="9.png", Price=1200, AvailableStock=75,RestockThreshold=10, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[2].Id, CatalogBrandId = Brands[1].Id, Name=".NET Foundation Sheet", Description = ".NET Foundation Sheet", Picture="10.png", Price=1200, AvailableStock=11, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[2].Id, CatalogBrandId = Brands[1].Id, Name="Cup<T> Sheet", Description = "Cup<T> Sheet", Picture="11.png", Price=850, AvailableStock=3, RestockThreshold=5, OnReorder=false },
            new Types.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Prism White TShirt", Description = "Prism White TShirt", Picture="12.png", Price=1200, AvailableStock=0, RestockThreshold=10, OnReorder=false },
        };

        public static async Task Seed(IMessageHandlerContext ctx)
        {
            var saga = ctx.Saga(Guid.NewGuid());

            // Create types
            foreach (var type in Types)
            {
                saga.Command(new eShop.Catalog.CatalogType.Commands.Define
                {
                    TypeId = type.Id,
                    Type = type.Type,
                });
            }
            await saga.Start().ConfigureAwait(false);

            var saga2 = ctx.Saga(Guid.NewGuid());
            // Create brands
            foreach (var brand in Brands)
            {
                saga2.Command(new eShop.Catalog.CatalogBrand.Commands.Define
                {
                    BrandId = brand.Id,
                    Brand = brand.Brand,
                });
            }
            await saga2.Start().ConfigureAwait(false);

            var assembly = Assembly.GetExecutingAssembly();
            var saga3 = ctx.Saga(Guid.NewGuid());
            // Create products
            foreach (var product in Products)
            {
                saga3.Command(new eShop.Catalog.Product.Commands.Add
                {
                    ProductId = product.Id,
                    CatalogBrandId = product.CatalogBrandId,
                    CatalogTypeId = product.CatalogTypeId,
                    Name = product.Name,
                    Price = product.Price
                }).Command(new eShop.Catalog.Product.Commands.UpdateDescription
                {
                    ProductId = product.Id,
                    Description = product.Description
                });

                if (product.AvailableStock > 0)
                    saga3.Command(new eShop.Catalog.Product.Commands.UpdateStock
                    {
                        ProductId = product.Id,
                        Stock = product.AvailableStock
                    });
                if (product.RestockThreshold > 0 || product.MaxStockThreshold > 0)
                    saga3.Command(new eShop.Catalog.Product.Commands.UpdateThresholds
                    {
                        ProductId = product.Id,
                        RestockThreshold = product.RestockThreshold,
                        MaxStockThreshold = product.MaxStockThreshold
                    });

                if (product.OnReorder)
                    saga3.Command(new eShop.Catalog.Product.Commands.MarkReordered
                    {
                        ProductId = product.Id,
                    });

                var stream = assembly.GetManifestResourceStream($"eShop.Configuration.Setup.Entities.Catalog.Pics.{product.Picture}");
                using (var memory = new MemoryStream())
                {
                    await stream.CopyToAsync(memory).ConfigureAwait(false);

                    saga3.Command(new eShop.Catalog.Product.Commands.SetPicture
                    {
                        ProductId = product.Id,
                        Content = Convert.ToBase64String(memory.ToArray()),
                        ContentType = "image/png"
                    });
                }
            }
            await saga3.Start().ConfigureAwait(false);
        }
    }
}
