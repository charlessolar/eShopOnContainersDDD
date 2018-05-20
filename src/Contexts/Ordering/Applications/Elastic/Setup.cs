using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Nest;

namespace eShop.Ordering
{
    [Category("Ordering")]
    public class Setup : ISetup
    {
        private readonly IElasticClient _client;

        public Setup(IElasticClient client)
        {
            _client = client;
        }

        public async Task<bool> Initialize()
        {
            await _client.CreateIndexAsync(typeof(Buyer.Models.OrderingBuyerIndex).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                )
                .Mappings(mappings => mappings.Map<Buyer.Models.OrderingBuyerIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.GivenName).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Boolean(s => s.Name(x => x.GoodStanding))
                            .Number(s => s.Name(x => x.TotalSpent).Type(NumberType.Long))
                            .Keyword(s => s.Name(x => x.PreferredCity).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PreferredState).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PreferredCountry).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PreferredZipCode).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PreferredPaymentCardholder).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PreferredPaymentMethod).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PreferredPaymentExpiration).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Buyer.Entities.Address.Models.Address).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                )
                .Mappings(mappings => mappings.Map<Buyer.Entities.Address.Models.Address>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.UserName).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.Alias).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.Street).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.City).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.State).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.Country).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ZipCode).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Buyer.Entities.PaymentMethod.Models.PaymentMethod).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                )
                .Mappings(mappings => mappings.Map<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.UserName).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.Alias).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.CardNumber).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.SecurityNumber).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.CardholderName).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Date(s => s.Name(x => x.Expiration))
                            .Keyword(s => s.Name(x => x.CardType).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);


            await _client.CreateIndexAsync(typeof(Order.Models.OrderingOrderIndex).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                )
                .Mappings(mappings => mappings.Map<Order.Models.OrderingOrderIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.Status).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.StatusDescription).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.BillingAddressId).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.BillingAddress).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.BillingCityState).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.BillingZipCode).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.BillingCountry).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ShippingAddressId).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ShippingAddress).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ShippingCityState).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ShippingZipCode).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ShippingCountry).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PaymentMethodId).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.PaymentMethod).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name(x => x.TotalItems).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.TotalQuantity).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.SubTotal).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Additional).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Total).Type(NumberType.Long))
                            .Boolean(s => s.Name(x => x.Paid))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Order.Entities.Item.Models.OrderingOrderItem).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                )
                .Mappings(mappings => mappings.Map<Order.Entities.Item.Models.OrderingOrderItem>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.OrderId).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ProductId).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Binary(s => s.Name(x => x.ProductPictureContents))
                            .Keyword(s => s.Name(x => x.ProductPictureContentType).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ProductName).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name(x => x.ProductDescription).IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name(x => x.ProductPrice).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Price).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Quantity).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.SubTotal).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.AdditionalFees).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.AdditionalTaxes).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Total).Type(NumberType.Long))
                    )))).ConfigureAwait(false);
            this.Done = true;
            return true;
        }

        public bool Done { get; private set; }
    }
}