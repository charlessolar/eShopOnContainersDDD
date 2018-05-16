using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Nest;

namespace eShop.Catalog
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
            await _client.CreateIndexAsync(typeof(CatalogBrand.Models.CatalogBrand).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<CatalogBrand.Models.CatalogBrand>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Brand").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(CatalogType.Models.CatalogType).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<CatalogType.Models.CatalogType>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Type").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(Product.Models.CatalogProductIndex).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<Product.Models.CatalogProductIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Name").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Description").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Type(NumberType.Long).Name("Price").IgnoreMalformed())
                            .Keyword(s => s.Name("CatalogTypeId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CatalogType").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CatalogBrandId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("CatalogBrand").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Type(NumberType.Double).Name("AvailableStock").IgnoreMalformed())
                            .Number(s => s.Type(NumberType.Double).Name("RestockThreshold").IgnoreMalformed())
                            .Number(s => s.Type(NumberType.Double).Name("MaxStockThreshold").IgnoreMalformed())
                            .Boolean(s => s.Name("OnReorder"))
                            .Binary(s => s.Name("PictureContents"))
                            .Keyword(s => s.Name("PictureContentType").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            this.Done = true;
            return true;
        }

        public bool Done { get; private set; }
    }
}