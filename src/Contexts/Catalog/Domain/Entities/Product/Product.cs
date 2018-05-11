using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Catalog.Product
{
    public class Product : Aggregates.Entity<Product,State>
    {
        private Product() { }

        public void Add(string name, int price, CatalogBrand.State brand, CatalogType.State type)
        {
            Apply<Events.Added>(x =>
            {
                x.ProductId = Id;
                x.Name = name;
                x.Price = price;
                x.CatalogBrandId = brand.Id;
                x.CatalogTypeId = type.Id;
            });
        }

        public void SetPicture(byte[] content, string contentType)
        {
            Apply<Events.PictureSet>(x =>
            {
                x.ProductId = Id;
                x.Content = content;
                x.ContentType = contentType;
            });
        }

        public void UpdateDescription(string description)
        {
            Apply<Events.DescriptionUpdated>(x =>
            {
                x.ProductId = Id;
                x.Description = description;
            });
        }

        public void UpdatePrice(int price)
        {
            Apply<Events.PriceUpdated>(x =>
            {
                x.ProductId = Id;
                x.Price = price;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.ProductId = Id;
            });
        }

        public void UpdateStock(decimal stock)
        {
            Apply<Events.StockUpdated>(x =>
            {
                x.ProductId = Id;
                x.Stock = stock;
            });
        }

        public void MarkReordered()
        {
            Apply<Events.ReorderMarked>(x => { x.ProductId = Id; });
        }

        public void UnMarkReordered()
        {
            Apply<Events.ReorderUnMarked>(x => { x.ProductId = Id; });
        }
    }
}
