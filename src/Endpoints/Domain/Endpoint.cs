using Aggregates;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FluentValidation;
using Infrastructure.Validation;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Serilog;
using Serilog;
using StructureMap;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace eShop
{
    internal class Program
    {
        private static IConfiguration Configuration { get; set; }

        static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);
        private static IContainer _container;
        private static IEndpointInstance _bus;

        private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.ExceptionObject as Exception, "<{EventId:l}> Unhandled exception", "Unhandled");
#if DEBUG
            Console.WriteLine("");
            Console.WriteLine("FATAL ERROR - Press return to close...");
            Console.ReadLine();
#endif
            Environment.Exit(1);
        }


        private static void Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddEnvironmentVariables("ESHOP_");

            Configuration = configurationBuilder.Build();

            Console.Title = "Domain";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
               .WriteTo.Console(outputTemplate: "[{Level}] {Message}{NewLine}{Exception}")
               .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            NServiceBus.Logging.LogManager.Use<SerilogFactory>();

            _container = new Container(x =>
            {
                x.For<IValidatorFactory>().Use<StructureMapValidatorFactory>();

                x.Scan(y =>
                {
                    y.TheCallingAssembly();

                    y.WithDefaultConventions();
                });
            });

            MutationManager.RegisterMutator("domain", typeof(Mutator));

            // Start the bus
            _bus = InitBus().Result;

            Console.WriteLine("Press CTRL+C to exit...");
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            QuitEvent.WaitOne();

            _bus.Stop().Wait();
        }
        private static async Task<IEndpointInstance> InitBus()
        {
            var config = new EndpointConfiguration("domain");

            // Configure RabbitMQ transport
            var transport = config.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString(GetRabbitConnectionString());

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));

            config.Pipeline.Remove("LogErrorOnInvalidLicense");

            var client = GetEventStore();

            await client.ConnectAsync().ConfigureAwait(false);

            await Aggregates.Configuration.Build(c => c
                .StructureMap(_container)
                .EventStore(new[] { client })
                .NewtonsoftJson()
                .NServiceBus(config)
                .SetUniqueAddress(Defaults.Instance.ToString())
                .SetRetries(20)
            ).ConfigureAwait(false);

            return Aggregates.Bus.Instance;
        }
        private static string GetRabbitConnectionString()
        {
            var host = Configuration["RabbitConnection"];
            var user = Configuration["RabbitUserName"];
            var password = Configuration["RabbitPassword"];

            if (string.IsNullOrEmpty(user))
                return $"host={host}";

            return $"host={host};username={user};password={password};";
        }
        private static IEventStoreConnection GetEventStore()
        {
            var host = Configuration["EventStoreConnection"];
            var user = Configuration["EventStoreUserName"];
            var password = Configuration["EventStorePassword"];

            if (string.IsNullOrEmpty(user))
                user = "admin";
            if (string.IsNullOrEmpty(password))
                password = "changeit";

            var endpoint = GetIPEndPointFromHostName(host, 1113);
            var cred = new UserCredentials(user, password);
            var settings = EventStore.ClientAPI.ConnectionSettings.Create()
                .KeepReconnecting()
                .KeepRetrying()
                .SetGossipSeedEndPoints(endpoint)
                .SetClusterGossipPort(2113)
                .SetHeartbeatInterval(TimeSpan.FromSeconds(30))
                .SetGossipTimeout(TimeSpan.FromMinutes(5))
                .SetHeartbeatTimeout(TimeSpan.FromMinutes(5))
                .SetTimeoutCheckPeriodTo(TimeSpan.FromMinutes(1))
                .SetDefaultUserCredentials(cred);

            return EventStoreConnection.Create(settings, endpoint, "Domain");
        }
        // https://stackoverflow.com/a/2101787/223547
        public static IPEndPoint GetIPEndPointFromHostName(string hostName, int port, bool throwIfMoreThanOneIP = false)
        {
            var addresses = System.Net.Dns.GetHostAddresses(hostName);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.",
                    "hostName"
                );
            }
            else if (throwIfMoreThanOneIP && addresses.Length > 1)
            {
                throw new ArgumentException(
                    "There is more that one IP address to the specified host.",
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], port); // Port gets validated here.
        }
    }
}
