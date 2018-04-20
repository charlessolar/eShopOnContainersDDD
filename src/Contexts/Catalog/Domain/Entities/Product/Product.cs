using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Catalog.Product
{
    public class Product : Aggregates.Entity<Product,State>
    {
        private Product() { }

        public void Add(string name, decimal price, CategoryBrand.State brand, CategoryType.State type)
        {
            Apply<Events.Added>(x =>
            {
                x.ProductId = Id;
                x.Name = name;
                x.Price = price;
                x.CategoryBrandId = brand.Id;
                x.CategoryTypeId = type.Id;
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

        public void UpdatePrice(decimal price)
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
    }
}
