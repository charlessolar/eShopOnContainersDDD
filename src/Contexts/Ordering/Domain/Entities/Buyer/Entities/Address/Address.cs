using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Buyer.Entities.Address
{
    public class Address : Aggregates.Entity<Address, State, Buyer>
    {
        private Address() { }

        public void Add(string street, string city, string state, string country, string zipCode)
        {
            Apply<Events.Added>(x =>
            {
                x.BuyerId = Parent.Id;
                x.AddressId = Id;
                x.Street = street;
                x.City = city;
                x.State = state;
                x.Country = country;
                x.ZipCode = zipCode;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(x =>
            {
                x.BuyerId = Parent.Id;
                x.AddressId = Id;
            });
        }
    }
}
