using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Nest;

namespace eShop.Basket
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

    [Category("Basket")]
    public class Setup : ISetup
    {
        private readonly IElasticClient _client;

        public Setup(IElasticClient client)
        {
            _client = client;
        }

        public async Task<bool> Initialize()
        {
            await _client.CreateIndexAsync(typeof(Basket.Models.BasketIndex).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Basket.Models.BasketIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.CustomerId).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.Customer).Fields(x => x.AutoCompleteFields()))
                            .Number(s => s.Name(x => x.TotalItems).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.TotalQuantity).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.SubTotal).Type(NumberType.Long))
                            .Date(s => s.Name(x => x.Created).Format("epoch_millis"))
                            .Date(s => s.Name(x => x.Updated).Format("epoch_millis"))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Basket.Entities.Item.Models.BasketItemIndex).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Basket.Entities.Item.Models.BasketItemIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.BasketId).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.ProductId).IgnoreAbove(256))
                            .Binary(s => s.Name(x => x.ProductPictureContents))
                            .Keyword(s => s.Name(x => x.ProductPictureContentType).IgnoreAbove(256))
                            .Text(s => s.Name(x => x.ProductName).Fields(x => x.AutoCompleteFields()))
                            .Text(s => s.Name(x => x.ProductDescription).Fields(x => x.AutoCompleteFields()))
                            .Number(s => s.Name(x => x.ProductPrice).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.Quantity).Type(NumberType.Long))
                            .Number(s => s.Name(x => x.SubTotal).Type(NumberType.Long))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Basket.Entities.Item.Services.ItemsInBasketHandler.ItemsBasket).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Basket.Entities.Item.Services.ItemsInBasketHandler.ItemsBasket>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.Items).IgnoreAbove(256))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Basket.Entities.Item.Services.ItemsUsingProductHandler.ProductItems).FullName.ToLower(), i => i
                .Settings(s => s
                    .NumberOfShards(3)
                    .TotalShardsPerNode(3)
                    .NumberOfReplicas(0)
                    .Analysis(analysis => analysis.AutoCompleteAnalyzers())
                )
                .Mappings(mappings => mappings.Map<Basket.Entities.Item.Services.ItemsUsingProductHandler.ProductItems>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name(x => x.Id).IgnoreAbove(256))
                            .Keyword(s => s.Name(x => x.Baskets).IgnoreAbove(256))
                    )))).ConfigureAwait(false);
            this.Done = true;
            return true;
        }

        public bool Done { get; private set; }
    }
}