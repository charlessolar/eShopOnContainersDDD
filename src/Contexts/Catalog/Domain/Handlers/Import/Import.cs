using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using NServiceBus;

namespace eShop.Catalog.Import
{
    [Category("Catalog")]
    public class Import : ISeed
    {

        private readonly IMessageSession _bus;

        public Import(IMessageSession bus)
        {
            _bus = bus;
        }

        public async Task<bool> Seed()
        {
            // Create types
            foreach (var type in Types)
            {
                await _bus.CommandToDomain(new CatalogType.Commands.Define
                {
                    TypeId = type.Id,
                    Type = type.Type,
                }).ConfigureAwait(false);
            }

            // Create brands
            foreach (var brand in Brands)
            {
                await _bus.CommandToDomain(new CatalogBrand.Commands.Define
                {
                    BrandId = brand.Id,
                    Brand = brand.Brand,
                }).ConfigureAwait(false);
            }

            var assembly = Assembly.GetExecutingAssembly();
            // Create products
            foreach (var product in Products)
            {
                await _bus.CommandToDomain(new Product.Commands.Add
                {
                    ProductId = product.Id,
                    CatalogBrandId = product.CatalogBrandId,
                    CatalogTypeId = product.CatalogTypeId,
                    Name = product.Name,
                    Price = product.Price
                }).ConfigureAwait(false);

                await _bus.CommandToDomain(new Product.Commands.UpdateDescription
                {
                    ProductId = product.Id,
                    Description = product.Description
                }).ConfigureAwait(false);


                await _bus.CommandToDomain(new Product.Commands.UpdateStock
                {
                    ProductId = product.Id,
                    Stock = product.AvailableStock
                }).ConfigureAwait(false);

                if(product.OnReorder)
                    await _bus.CommandToDomain(new Product.Commands.MarkReordered
                    {
                        ProductId = product.Id,
                    }).ConfigureAwait(false);

                var stream = assembly.GetManifestResourceStream($"eShop.Catalog.Import.Pics.{product.Picture}");
                using (var memory = new MemoryStream())
                {
                    await stream.CopyToAsync(memory).ConfigureAwait(false);

                    await _bus.CommandToDomain(new Product.Commands.SetPicture
                    {
                        ProductId = product.Id,
                        Content = memory.ToArray(),
                        ContentType = "image/png"
                    }).ConfigureAwait(false);
                }
            }
            
            this.Started = true;
            return true;
        }

        public bool Started { get; private set; }



        private static readonly Models.CatalogBrand[] Brands = new[]
        {
            new Models.CatalogBrand {Id = Guid.NewGuid(), Brand = "Azure"},
            new Models.CatalogBrand {Id = Guid.NewGuid(), Brand = ".NET"},
            new Models.CatalogBrand {Id = Guid.NewGuid(), Brand = "Visual Studio"},
            new Models.CatalogBrand {Id = Guid.NewGuid(), Brand = "SQL Server"},
            new Models.CatalogBrand {Id = Guid.NewGuid(), Brand = "Other"},
        };

        private static readonly Models.CatalogType[] Types = new[]
        {
            new Models.CatalogType {Id = Guid.NewGuid(), Type = "Mug"},
            new Models.CatalogType {Id = Guid.NewGuid(), Type = "T-Shirt"},
            new Models.CatalogType {Id = Guid.NewGuid(), Type = "Sheet"},
            new Models.CatalogType {Id = Guid.NewGuid(), Type = "USB Memory Stick"},
        };
        private static readonly Models.Product[] Products = new[]
        {
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[1].Id, Name=".NET Bot Black Hoodie", Description = ".NET Bot Black Hoodie, and more", Picture="1.png", Price=1950, AvailableStock=100, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[0].Id, CatalogBrandId = Brands[1].Id, Name=".NET Black & White Mug", Description = ".NET Black & White Mug", Picture="2.png", Price=850, AvailableStock=90, OnReorder=true },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Prism White T-Shirt", Description = "Prism White T-Shirt", Picture="3.png", Price=1200, AvailableStock=55, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[1].Id, Name=".NET Foundation T-shirt", Description = ".NET Foundation T-shirt", Picture="4.png", Price=1200, AvailableStock=120, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[2].Id, CatalogBrandId = Brands[4].Id, Name="Roslyn Red Sheet", Description = "Roslyn Red Sheet", Picture="5.png", Price=850, AvailableStock=55, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[1].Id, Name=".NET Blue Hoodie", Description = ".NET Blue Hoodie", Picture="6.png", Price=1200, AvailableStock=20, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Roslyn Red T-Shirt", Description = "Roslyn Red T-Shirt", Picture="7.png", Price=1200, AvailableStock=10, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Kudu Purple Hoodie", Description = "Kudu Purple Hoodie", Picture="8.png", Price=850, AvailableStock=35, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[0].Id, CatalogBrandId = Brands[4].Id, Name="Cup<T> White Mug", Description = "Cup<T> White Mug", Picture="9.png", Price=1200, AvailableStock=75, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[2].Id, CatalogBrandId = Brands[1].Id, Name=".NET Foundation Sheet", Description = ".NET Foundation Sheet", Picture="10.png", Price=1200, AvailableStock=11, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[2].Id, CatalogBrandId = Brands[1].Id, Name="Cup<T> Sheet", Description = "Cup<T> Sheet", Picture="11.png", Price=850, AvailableStock=3, OnReorder=false },
            new Models.Product { Id = Guid.NewGuid(), CatalogTypeId = Types[1].Id, CatalogBrandId = Brands[4].Id, Name="Prism White TShirt", Description = "Prism White TShirt", Picture="12.png", Price=1200, AvailableStock=0, OnReorder=false },
        };
    }
}
