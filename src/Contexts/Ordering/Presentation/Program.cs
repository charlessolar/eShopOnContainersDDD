using Aggregates;
using Ordering.Extensions;
using Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Events;
using System.Net;

var configuration = GetConfiguration();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    var host = BuildWebHost(configuration, args);

    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IHost BuildWebHost(IConfiguration configuration, string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(CreateSerilogLogger);
    ConfigureNServiceBus(builder, configuration);
    builder.WebHost
        .CaptureStartupErrors(false)
        .ConfigureKestrel(options =>
        {
            var ports = GetDefinedPorts(configuration);
            options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            });

            options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });

        })
        .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
        .UseFailing(options =>
        {
            options.ConfigPath = "/Failing";
            options.NotFilteredPaths.AddRange(new[] { "/hc", "/liveness" });
        })
        .UseStartup<Startup>()
        .UseContentRoot(Directory.GetCurrentDirectory());

    var host = builder.Build();
    host.UseSerilogRequestLogging();
    return host;
}
void ConfigureNServiceBus(WebApplicationBuilder builder, IConfiguration configuration)
{

    var endpointConfiguration = new EndpointConfiguration("Web");

    var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
    transport.UseConventionalRoutingTopology(QueueType.Classic);
    transport.ConnectionString(GetRabbitConnectionString(configuration));

    endpointConfiguration.Pipeline.Register(
                behavior: typeof(IncomingLoggingMessageBehavior),
                description: "Logs incoming messages"
            );
    endpointConfiguration.Pipeline.Register(
                behavior: typeof(OutgoingLoggingMessageBehavior),
                description: "Logs outgoing messages"
            );

    builder.Host
        .AddAggregatesNet(c => c
                .NewtonsoftJson()
                .NServiceBus(endpointConfiguration)
                .SetCommandDestination("Domain"));
}
string GetRabbitConnectionString(IConfiguration config)
{
    var host = config["RabbitConnection"];
    var user = config["RabbitUserName"];
    var password = config["RabbitPassword"];

    if (string.IsNullOrEmpty(user))
        return $"host={host}";

    return $"host={host};username={user};password={password};";
}

void CreateSerilogLogger(HostBuilderContext context, IServiceProvider services, LoggerConfiguration logConfiguration)
{
    var configuration = context.Configuration;
    var seqServerUrl = configuration["Serilog:SeqServerUrl"];
    var logstashUrl = configuration["Serilog:LogstashgUrl"];
    logConfiguration
        .MinimumLevel.Verbose()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .ReadFrom.Configuration(configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
        .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl, queueLimitBytes: null);

}

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    var config = builder.Build();

    return builder.Build();
}

(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
{
    var grpcPort = config.GetValue("GRPC_PORT", 5001);
    var port = config.GetValue("PORT", 80);
    return (port, grpcPort);
}
public partial class Program
{

    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}