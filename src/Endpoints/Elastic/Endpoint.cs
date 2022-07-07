using Aggregates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Infrastructure;
using System.Threading.Tasks;
using System;
using System.Threading;
using Nest;
using Elasticsearch.Net;
using System.Text.RegularExpressions;
using Nest.JsonNetSerializer;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddEnvironmentVariables("ESHOP_");

var configuration = configurationBuilder.Build();

var endpointConfiguration = new EndpointConfiguration("Elastic");

endpointConfiguration.UsePersistence<InMemoryPersistence>();
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseConventionalRoutingTopology();
transport.ConnectionString(GetRabbitConnectionString(configuration));

endpointConfiguration.Pipeline.Register(
            behavior: typeof(IncomingLoggingMessageBehavior),
            description: "Logs incoming messages"
        );
endpointConfiguration.Pipeline.Register(
            behavior: typeof(OutgoingLoggingMessageBehavior),
            description: "Logs outgoing messages"
        );


IElasticClient client = GetElastic(configuration);
var host = Host.CreateDefaultBuilder(args)  
    .UseConsoleLifetime()
    .AddAggregatesNet(c => c
            .EventStore(es => es.AddClient(GetEventStoreConnectionString(configuration), "Elastic"))
            .NewtonsoftJson()
            .Application<eShop.UnitOfWork>()
            .NServiceBus(endpointConfiguration)
            .SetCommandDestination("Domain"))
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton(client);
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddConfiguration(context.Configuration.GetSection("Logging"));
        });
    }).Build();


await host.RunAsync();


static string GetRabbitConnectionString(IConfiguration config)
{
    var host = config["RabbitConnection"];
    var user = config["RabbitUserName"];
    var password = config["RabbitPassword"];

    if (string.IsNullOrEmpty(user))
        return $"host={host}";

    return $"host={host};username={user};password={password};";
}
static string GetEventStoreConnectionString(IConfiguration config)
{
    var host = config["EventStoreConnection"];
    var user = config["EventStoreUserName"] ?? "admin";
    var password = config["EventStorePassword"] ?? "changeit";
    return $"esdb://{user}:{password}@{host}?tls=false";
}
static IElasticClient GetElastic(IConfiguration config)
{
    var url = config["ElasticConnection"];
    var user = config["ElasticUserName"];
    var password = config["ElasticPassword"];

    var node = new Uri(url);
    var pool = new SingleNodeConnectionPool(node);

    var regex = new Regex("([+\\-!\\(\\){}\\[\\]^\"~*?:\\\\\\/>< ]|[&\\|]{2}|AND|OR|NOT)", RegexOptions.Compiled);
    var settings = new Nest.ConnectionSettings(pool, (builtin, param) => new JsonNetSerializer(builtin, param))
        .BasicAuthentication(user, password)
        .DefaultIndex("eShop")
        .DefaultFieldNameInferrer(field => regex.Replace(field, ""))
        .DisableDirectStreaming();

    return new ElasticClient(settings);
}

