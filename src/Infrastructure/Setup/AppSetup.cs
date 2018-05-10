using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Setup.Attributes;
using Serilog;

namespace Infrastructure.Setup
{
    public static class AppSetup
    {
        public static void InitiateSetup(IEnumerable<ISetup> setups)
        {

            Setups = setups.Select(o =>
            {
                var depends = o.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;
                var category = o.GetType().GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                return new SetupInfo
                {
                    Name = o.GetType().Name,
                    Depends = depends == null ? new string[] { } : depends.Depends,
                    Category = category.Name,
                    Operation = o
                };
            }).ToList();
        }

        public static IEnumerable<SetupInfo> Setups { get; private set; }

        public static ISetup GetSetup(string operation)
        {
            return Setups.SingleOrDefault(o => o.Name == operation)?.Operation;
        }

        public static Task SetupApplication()
        {
            return SetupApplication(null);
        }

        private static async Task SetupApplication(SetupInfo info)
        {
            var _logger = Log.Logger.With<SetupInfo>();

            var watch = new Stopwatch();

            // Depends will be either, ALL setup operations if info is null, or all the operations info depends on
            var depends = Setups;
            if (info != null)
                depends = depends.Where(x => info.Depends.Any() && info.Depends.Contains(x.Name));

            foreach (var depend in depends)
                await SetupApplication(depend).ConfigureAwait(false);

            if (info == null || info.Operation.Done)
                return;

            _logger.Information("**************************************************************");
            _logger.InfoEvent("SetupStart", "   Running setup operation {Category}.{Name}", info.Category, info.Name);
            _logger.Information("**************************************************************");

            watch.Start();
            if (!await info.Operation.Initialize().ConfigureAwait(false))
            {
                _logger.Information("ERROR - Failed to complete setup operation!");
                Environment.Exit(1);
            }
            watch.Stop();
            _logger.Information("**************************************************************");
            _logger.InfoEvent("SetupComplete", "   Finished operation {Category}.{Name} in {Elapsed}!", info.Category, info.Name, watch.Elapsed);
            _logger.Information("**************************************************************");
        }
    }

    public class SetupInfo
    {
        public string Name { get; set; }

        public string[] Depends { get; set; }

        public string Category { get; set; }

        public ISetup Operation { get; set; }
    }
}
