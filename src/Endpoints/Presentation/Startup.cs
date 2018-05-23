using Aggregates;
using NServiceBus;
using NServiceBus.Features;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Text;
using ServiceStack.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using ServiceStack.Redis;
using ServiceStack.Auth;
using System.Threading.Tasks;
using LogManager = ServiceStack.Logging.LogManager;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Serilog;
using App.Metrics.Health;
using StructureMap;
using App.Metrics.Health.Builder;
using eShop.Presentation.Authentication;
using Infrastructure.Logging;
using ServiceStack.Api.OpenApi;
using ServiceStack.Seq.RequestLogsFeature;

namespace eShop
{
    class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => builder.AddEnvironmentVariables("ESHOP_"))
                .UseStartup<Startup>()
                .Build();
    }

    class Startup
    {
        IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    class AppHost : AppHostBase
    {
        private static StructureMap.IContainer _container;
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

        //Tell Service Stack the name of your application and where to find your web services
        public AppHost()
            : base("Api", typeof(AppHost).Assembly)
        {
        }

        public override ServiceStackHost Init()
        {
            var config = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext().Enrich.WithThreadId().Enrich.WithProcessName().Enrich.WithProperty("Endpoint", "ServiceStack")
                .WriteTo.Console(outputTemplate: "[{Level}] {Message}{NewLine}{Exception}",
                    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning);

            if (!string.IsNullOrEmpty(AppSettings.Get<string>("SeqConnection")))
                config.WriteTo.Seq(AppSettings.Get<string>("SeqConnection"));

            Log.Logger = config.CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;

            _container = new Container(x =>
            {
                x.For<IMessageSession>().Use(() => Aggregates.Bus.Instance);
                x.For<Mutator>().Use<Mutator>();

                x.Scan(y =>
                {
                    y.TheCallingAssembly();

                    y.WithDefaultConventions();
                });
            });

            MutationManager.RegisterMutator("commands", typeof(Mutator));

            _bus = InitBus().Result;

            return base.Init();
        }
        private async Task<IEndpointInstance> InitBus()
        {
            var config = new EndpointConfiguration("presentation");

            // Configure RabbitMQ transport
            var transport = config.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString(GetRabbitConnectionString());

            config.UsePersistence<InMemoryPersistence>();
            config.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));

            config.Pipeline.Register<LogIncomingMessageRegistration>();
            config.Pipeline.Remove("LogErrorOnInvalidLicense");

            await Aggregates.Configuration.Build(c => c
                .StructureMap(_container)
                .NewtonsoftJson()
                .NServiceBus(config)
                .SetUniqueAddress(Defaults.Instance.ToString())
                .SetPassive()
                .SetRetries(20)
            ).ConfigureAwait(false);

            return Aggregates.Bus.Instance;
        }
        private string GetRabbitConnectionString()
        {
            var host = AppSettings.Get<string>("RabbitConnection");
            var user = AppSettings.Get<string>("RabbitUserName", "");
            var password = AppSettings.Get<string>("RabbitPassword", "");

            if (string.IsNullOrEmpty(user))
                return $"host={host}";

            return $"host={host};username={user};password={password};";
        }

        public override void Configure(Funq.Container container)
        {
            JsConfig.AlwaysUseUtc = true;
            JsConfig.AssumeUtc = true;
            JsConfig.TreatEnumAsInteger = true;
            JsConfig.IncludeNullValues = false;
            JsConfig.DateHandler = DateHandler.ISO8601;

            if (!AppSettings.Exists("ServiceStackLicense"))
                throw new ArgumentException("You must define ServiceStackLicense in docker-compose.override.yml");
            Licensing.RegisterLicense(AppSettings.Get<string>("ServiceStackLicense"));

            ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                Log.Logger.Error(exception, "Service exception {ExceptionType} - {ExceptionMessage} on request {Request}", exception.GetType().Name, exception.Message, httpReq);
                return null;
            });

            UncaughtExceptionHandlers.Add((req, res, operationName, ex) =>
            {
                Log.Logger.Error(ex, "Unhandled exception {ExceptionType} - {ExceptionMessage}, operation {OperationName} on request {Request}", ex.GetType().Name, ex.Message, operationName, req);
            });

            // Todo: see: http://docs.servicestack.net/releases/v4.5.10#vulnerability-with-object-properties
            JsConfig.AllowRuntimeType = _ => true;

            SetConfig(new HostConfig { DebugMode = true, ApiVersion = "1" });

            container.Adapter = new StructureMapContainerAdapter(_container);

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[]
                {
                    new JwtAuthProvider(AppSettings) {
                        AuthKeyBase64 = AppSettings.GetString("jwt.AuthKeyBase64"),
                        RequireSecureConnection=false },
                    new CredentialsAuthProvider()
                }));
            Plugins.Add(new RegistrationFeature());
            container.Register<IUserSessionSource>(x => new IdentitySessionSource(_bus));
            container.Register<IUserAuthRepository>(x => new AuthRepository(_bus));

            Plugins.Add(new OpenApiFeature());
            if (!string.IsNullOrEmpty(AppSettings.GetString("SeqConnection")))
            {
                Plugins.Add(new SeqRequestLogsFeature
                {
                    AppendProperties = (request, dto, response, duration) => new Dictionary<string, object> { ["Endpoint"] = "ServiceStack", ["EventId"] = "Request" },
                    SeqUrl = AppSettings.GetString("SeqConnection")
                });
            }

            Plugins.Add(new PostmanFeature
            {
                DefaultLabelFmt = new List<string> { "type: english", " ", "route" }
            });

            var hostServer = AppSettings.GetString("HostServer");
            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[]
                {
                    string.IsNullOrEmpty(hostServer) ? "http://localhost:9000" : hostServer
                },
                allowCredentials: true,
                allowedHeaders: "Content-Type, Authorization",
                allowedMethods: "GET, POST, DELETE, OPTIONS"
            ));

            Plugins.Add(new ValidationFeature());
            Plugins.Add(new Basket.Plugin());
            Plugins.Add(new Catalog.Plugin());
            Plugins.Add(new Identity.Plugin());
            Plugins.Add(new Location.Plugin());
            Plugins.Add(new Marketing.Plugin());
            Plugins.Add(new Ordering.Plugin());
            Plugins.Add(new Payment.Plugin());
            Plugins.Add(new Configuration.Plugin());

            var nativeTypes = this.GetPlugin<NativeTypesFeature>();
            nativeTypes.MetadataTypesConfig.GlobalNamespace = "DTOs";

        }
    }
}

