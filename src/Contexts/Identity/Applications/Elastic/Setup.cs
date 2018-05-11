using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Nest;

namespace eShop.Identity
{
    [Category("Identity")]
    public class Setup : ISetup
    {
        private readonly IElasticClient _client;

        public Setup(IElasticClient client)
        {
            _client = client;
        }
        public async Task<bool> Initialize()
        {

            await _client.CreateIndexAsync(typeof(Role.Models.RoleIndex).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<Role.Models.RoleIndex>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Name").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name("Users").Type(NumberType.Long).IgnoreMalformed())
                            .Boolean(s => s.Name("Disabled"))
                    )))).ConfigureAwait(false);


            await _client.CreateIndexAsync(typeof(User.Models.User).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<User.Models.User>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("Id").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("GivenName").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Boolean(s => s.Name("Disabled"))
                            .Keyword(s => s.Name("Roles").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Number(s => s.Name("LastLogin").Type(NumberType.Long).IgnoreMalformed())
                    )))).ConfigureAwait(false);

            await _client.CreateIndexAsync(typeof(User.Services.UsersWithRoleHandler.UserRoles).FullName.ToLower(), i => i
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
                .Mappings(mappings => mappings.Map<User.Models.User>(map =>
                    map.Properties(props =>
                        props.Keyword(s => s.Name("RoleId").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                            .Keyword(s => s.Name("Users").IgnoreAbove(256).Norms(false).IndexOptions(IndexOptions.Docs))
                    )))).ConfigureAwait(false);

            this.Done = true;
            return true;
        }

        public bool Done { get; private set; }
    }
}