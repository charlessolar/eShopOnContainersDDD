using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Nest;

namespace eShop.Basket
{
    [Category("Catalog")]
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
                    .Analysis(analysis => analysis
                        .TokenFilters(f => f.NGram("ngram", d => d.MinGram(3).MaxGram(4)))
                        .Analyzers(a => a
                            .Custom("default",
                                t => t.Tokenizer("keyword").Filters(new[]
                                    {"standard", "lowercase", "asciifolding", "kstem", "ngram"}))
                        )
                    )
                )
                .Mappings(mappings => mappings.Map<Basket.Models.BasketIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CustomerId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Customer").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name("TotalQuantity").Type(NumberType.Long))
                            .Number(s => s.Name("SubTotal").Type(NumberType.Long))
                            .Number(s => s.Name("ExtraTotal").Type(NumberType.Long))
                            .Number(s => s.Name("Total").Type(NumberType.Long))
                            .Number(s => s.Name("Created").Type(NumberType.Long))
                            .Number(s => s.Name("Updated").Type(NumberType.Long))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Basket.Entities.Item.Models.ItemIndex).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<Basket.Entities.Item.Models.ItemIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("BasketId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Binary(s => s.Name("ProductPictureContents"))
                            .Keyword(s => s.Name("ProductPictureContentType").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ProductName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("ProductDescription").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name("ProductPrice").Type(NumberType.Long))
                            .Number(s => s.Name("Quantity").Type(NumberType.Long))
                            .Number(s => s.Name("Additional").Type(NumberType.Long))
                            .Number(s => s.Name("Total").Type(NumberType.Long))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Basket.Entities.Item.Services.ItemsInBasketHandler.ItemsBasket).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<Basket.Entities.Item.Services.ItemsInBasketHandler.ItemsBasket>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("BasketId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Items").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Basket.Entities.Item.Services.ItemsUsingProductHandler.ProductItems).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<Basket.Entities.Item.Services.ItemsUsingProductHandler.ProductItems>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("ProductId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Baskets").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);
            this.Done = true;
            return true;
        }

        public bool Done { get; private set; }
    }
}