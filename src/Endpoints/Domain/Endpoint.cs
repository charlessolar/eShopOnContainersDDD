using Aggregates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Infrastructure;
using System;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddEnvironmentVariables("ESHOP_");

var configuration = configurationBuilder.Build();

var endpointConfiguration = new EndpointConfiguration("Domain");

var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseConventionalRoutingTopology(QueueType.Quorum);
transport.ConnectionString(GetRabbitConnectionString(configuration));

endpointConfiguration.Pipeline.Register(
            behavior: typeof(IncomingLoggingMessageBehavior),
            description: "Logs incoming messages"
        );
endpointConfiguration.Pipeline.Register(
            behavior: typeof(OutgoingLoggingMessageBehavior),
            description: "Logs outgoing messages"
        );


var host = Host.CreateDefaultBuilder(args)
    .UseConsoleLifetime()
    .AddAggregatesNet(c => c
        .EventStore(es => es.AddClient(GetEventStoreConnectionString(configuration), "Domain"))
        .Domain()
        .NewtonsoftJson()
        .NServiceBus(endpointConfiguration)
        .SetCommandDestination("Domain"))
    .ConfigureServices((context, services) =>
    {
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

