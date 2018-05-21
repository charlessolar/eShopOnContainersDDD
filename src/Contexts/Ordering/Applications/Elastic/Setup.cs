using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Nest;

namespace eShop.Ordering
{
    static class Extensions
    {
        public static AnalysisDescriptor AutoCompleteAnalyzers(this AnalysisDescriptor analysis)
        {
            return analysis.Tokenizers(t => t.Whitespace("whitespace_tokenizer"))
                        .TokenFilters(t => t.EdgeNGram("ngram_filter", n => n.MinGram(1).MaxGram(8)))
                        .Analyzers(a => a
                            .Custom("default_autocomplete", c => c
                                .Tokenizer("whitespace_tokenizer").Filters("lowercase", "asciifolding")
                            )
                            .Custom("snowball_autocomplete", c => c
                                .Tokenizer("whitespace_tokenizer").Filters("lowercase", "asciifolding", "snowball")
                            )
                            .Custom("shingle_autocomplete", c => c
                                .Tokenizer("whitespace_tokenizer").Filters("shingle", "lowercase", "asciifolding")
                            )
                            .Custom("ngram_autocomplete", c => c
                                .Tokenizer("whitespace_tokenizer").Filters("lowercase", "asciifolding", "ngram_filter")
                            )
                            .Custom("search_autocomplete", c => c
                                .Tokenizer("whitespace_tokenizer").Filters("lowercase", "asciifolding")
                            )
                        );
        }
        public static PropertiesDescriptor<T> AutoCompleteFields<T>(this PropertiesDescriptor<T> descriptor) where T : class
        {
            return descriptor
                                .Text(sub => sub.Name("default").Analyzer("default_autocomplete"))
                                .Text(sub => sub.Name("stemmed").Analyzer("snowball_autocomplete"))
                                .Text(sub => sub.Name("shingles").Analyzer("shingle_autocomplete"))
                                .Text(sub => sub.Name("ngrams").Analyzer("ngram_autocomplete").SearchAnalyzer("search_autocomplete"));
        }
    }
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
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Buyer.Models.OrderingBuyerIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.GivenName).Fields(x => x.AutoCompleteFields()))
                            .Boolean(s => s.Name(x => x.GoodStanding))
                            .Number(s => s.Name(x => x.TotalSpent).Type(NumberType.Long))
                            .Text(s => s.Name(x => x.PreferredCity).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.PreferredState).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.PreferredCountry).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.PreferredZipCode).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.PreferredPaymentCardholder).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.PreferredPaymentMethod).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.PreferredPaymentExpiration).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.GivenName).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.GivenName).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.GivenName).Fields(x => x.AutoCompleteFields()))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Buyer.Entities.Address.Models.Address).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Buyer.Entities.Address.Models.Address>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.UserName).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.Alias).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.Street).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.City).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.State).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.Country).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ZipCode).Fields(x => x.AutoCompleteFields()))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Buyer.Entities.PaymentMethod.Models.PaymentMethod).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Buyer.Entities.PaymentMethod.Models.PaymentMethod>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.UserName).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.Alias).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.CardNumber).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.SecurityNumber).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.CardholderName).Fields(x => x.AutoCompleteFields()))
                            .Date(s => s.Name(x => x.Expiration))
                            .Text(s => s.Name(x => x.CardType).Fields(x => x.AutoCompleteFields()))
                    )))).ConfigureAwait(false);


            await _client.CreateIndexAsync(typeof(Order.Models.OrderingOrder).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Order.Models.OrderingOrder>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.Status).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.StatusDescription).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.BillingAddressId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.BillingAddress).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.BillingCityState).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.BillingZipCode).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.BillingCountry).Fields(x => x.AutoCompleteFields()))
                            .Keyword(s => s.Name(x => x.ShippingAddressId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.ShippingAddress).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ShippingCityState).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ShippingZipCode).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ShippingCountry).Fields(x => x.AutoCompleteFields()))
                            .Keyword(s => s.Name(x => x.PaymentMethodId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.PaymentMethod).Fields(x => x.AutoCompleteFields()))
                            .Number(s => s.Name(x => x.TotalItems).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.TotalQuantity).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.SubTotal).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.AdditionalFees).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.AdditionalTaxes).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Total).Type(NumberType.Long))
                            .Date(s => s.Name(x => x.Created).Format("epoch_millis"))
                            .Date(s => s.Name(x => x.Updated).Format("epoch_millis"))
                            .Boolean(s => s.Name(x => x.Paid))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Order.Models.OrderingOrderIndex).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Order.Models.OrderingOrderIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.Status).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.StatusDescription).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.BillingAddressId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.BillingAddress).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.BillingCityState).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.BillingZipCode).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.BillingCountry).Fields(x => x.AutoCompleteFields()))
                            .Keyword(s => s.Name(x => x.ShippingAddressId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.ShippingAddress).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ShippingCityState).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ShippingZipCode).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ShippingCountry).Fields(x => x.AutoCompleteFields()))
                            .Keyword(s => s.Name(x => x.PaymentMethodId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.PaymentMethod).Fields(x => x.AutoCompleteFields()))
                            .Number(s => s.Name(x => x.TotalItems).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.TotalQuantity).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.SubTotal).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Additional).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Total).Type(NumberType.Long))
                            .Date(s => s.Name(x => x.Created).Format("epoch_millis"))
                            .Date(s => s.Name(x => x.Updated).Format("epoch_millis"))
                            .Boolean(s => s.Name(x => x.Paid))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Order.Entities.Item.Models.OrderingOrderItem).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Order.Entities.Item.Models.OrderingOrderItem>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.OrderId).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.ProductId).IgnoreAbove(256))
                            .Binary(s => s.Name(x => x.ProductPictureContents))
                            .Keyword(s => s.Name(x => x.ProductPictureContentType).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.ProductName).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ProductDescription).Fields(x => x.AutoCompleteFields()))
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