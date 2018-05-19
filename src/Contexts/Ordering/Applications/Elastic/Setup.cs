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
                    .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(3).MaxGram(4)))
                        .Analyzers(a => a
                            .Custom("default",
                                t => t.Tokenizer("keyword").Filters(new[]
                                    {"standard", "lowercase", "asciifolding", "kstem", "ngram"}))
                        )
                    )
                )
                .Mappings(mappings => mappings.Map<Buyer.Models.OrderingBuyerIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("GivenName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Boolean(s => s.Name("GoodStanding"))
                            .Number(s => s.Name("TotalSpent").Type(NumberType.Long))
                            .Keyword(s => s.Name("PreferredCity").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PreferredState").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PreferredCountry").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PreferredZipCode").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PreferredPaymentCardholder").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PreferredPaymentMethod").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PreferredPaymentExpiration").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Buyer.Entities.Address.Models.Address).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(3).MaxGram(4)))
                        .Analyzers(a => a
                            .Custom("default",
                                t => t.Tokenizer("keyword").Filters(new[]
                                    {"standard", "lowercase", "asciifolding", "kstem", "ngram"}))
                        )
                    )
                )
                .Mappings(mappings => mappings.Map<Buyer.Entities.Address.Models.Address>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("UserName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Alias").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Street").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("City").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("State").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Country").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ZipCode").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Buyer.Entities.PaymentMethod.Models.PaymentMethod).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(3).MaxGram(4)))
                        .Analyzers(a => a
                            .Custom("default",
                                t => t.Tokenizer("keyword").Filters(new[]
                                    {"standard", "lowercase", "asciifolding", "kstem", "ngram"}))
                        )
                    )
                )
                .Mappings(mappings => mappings.Map<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("UserName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Alias").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CardNumber").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("SecurityNumber").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CardholderName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Date(s => s.Name("Expiration"))
                            .Keyword(s => s.Name("CardType").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);


            await _client.CreateIndexAsync(typeof(Order.Models.OrderingOrderIndex).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(3).MaxGram(4)))
                        .Analyzers(a => a
                            .Custom("default",
                                t => t.Tokenizer("keyword").Filters(new[]
                                    {"standard", "lowercase", "asciifolding", "kstem", "ngram"}))
                        )
                    )
                )
                .Mappings(mappings => mappings.Map<Order.Models.OrderingOrderIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Status").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("StatusDescription").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("AddressId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Address").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CityState").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ZipCode").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Country").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PaymentMethodId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("PaymentMethod").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name("TotalItems").Type(NumberType.Long))
                            .Number(s => s.Name("TotalQuantity").Type(NumberType.Long))
                            .Number(s => s.Name("SubTotal").Type(NumberType.Long))
                            .Number(s => s.Name("Additional").Type(NumberType.Long))
                            .Number(s => s.Name("Total").Type(NumberType.Long))
                            .Boolean(s => s.Name("Paid"))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Order.Entities.Item.Models.OrderingOrderItem).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(3).MaxGram(4)))
                        .Analyzers(a => a
                            .Custom("default",
                                t => t.Tokenizer("keyword").Filters(new[]
                                    {"standard", "lowercase", "asciifolding", "kstem", "ngram"}))
                        )
                    )
                )
                .Mappings(mappings => mappings.Map<Order.Entities.Item.Models.OrderingOrderItem>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("OrderId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ProductId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Binary(s => s.Name("ProductPictureContents"))
                            .Keyword(s => s.Name("ProductPictureContentType").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ProductName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ProductDescription").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name("ProductPrice").Type(NumberType.Long))
                            .Number(s => s.Name("Price").Type(NumberType.Long))
                            .Number(s => s.Name("Quantity").Type(NumberType.Long))
                            .Number(s => s.Name("SubTotal").Type(NumberType.Long))
                            .Number(s => s.Name("AdditionalFees").Type(NumberType.Long))
                            .Number(s => s.Name("AdditionalTaxes").Type(NumberType.Long))
                            .Number(s => s.Name("Total").Type(NumberType.Long))
                    )))).ConfigureAwait(false);
            this.Done = true;
            return true;
        }

        public bool Done { get; private set; }
    }
}