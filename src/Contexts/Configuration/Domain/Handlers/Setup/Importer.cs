using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Infrastructure.Setup;
using Infrastructure.Setup.Attributes;
using Serilog;
using StructureMap;

namespace eShop.Configuration.Setup
{
    internal class SeedInfo
    {
        public string Name { get; set; }

        public string[] Depends { get; set; }

        public string Category { get; set; }

        public ISeed Operation { get; set; }
    }
    public class Importer
    {
        private static ILogger Logger = Log.Logger.With<Importer>();

        private static IEnumerable<SeedInfo> _imports;

        public static void LoadOperations(IContainer container)
        {
            Logger.Debug("Loading all operations");
            if (_imports != null && _imports.Any())
                return;

            _imports = container.GetAllInstances<ISeed>().Select(o =>
            {
                var depends = o.GetType().GetCustomAttributes(typeof(DependsAttribute), true).FirstOrDefault() as DependsAttribute;
                var category = o.GetType().GetCustomAttributes(typeof(CategoryAttribute), true).FirstOrDefault() as CategoryAttribute;
                return new SeedInfo
                {
                    Name = o.GetType().Name,
                    Depends = depends?.Depends,
                    Category = category?.Name ?? "Unknown",
                    Operation = o,
                };
            }).ToList();
        }


        public static async Task<bool> ImportCategory(string category, bool depends = true)
        {
            var start = DateTime.UtcNow;

            Logger.Information("**************************************************************");
            Logger.InfoEvent("CategoryStart", "   Started importing {Category}", category);
            Logger.Information("**************************************************************");

            foreach (var info in _imports.Where(o => category == "*" || o.Category.ToLower() == category.ToLower()))
            {
                if (info.Operation.Started)
                    continue;

                if (!await RunImport(info, depends).ConfigureAwait(false))
                    Logger.Error($"Failed to run operation {info.Name}");
            }

            Logger.Information("**************************************************************");
            Logger.InfoEvent("CategoryEnd", "    Finished category {Category} in {Duration}", category, (DateTime.UtcNow - start));
            Logger.Information("**************************************************************");

            return true;
        }
        private static async Task<bool> RunImport(SeedInfo info, bool depends = true)
        {
            var start = DateTime.UtcNow;
            Logger.Information("**************************************************************");
            Logger.InfoEvent("OperationStart", "   Running operation {Name}", info.Name);
            Logger.Information("**************************************************************");

            if (!await info.Operation.Seed().ConfigureAwait(false))
            {
                Logger.WarnEvent("OperationError", "ERROR - Failed to run operation {Name}!", info.Name);
                return false;
            }

            Logger.Information("**************************************************************");
            Logger.InfoEvent("OperationEnd", "    Finished operation {Name} in {Duration}", info.Name, (DateTime.UtcNow - start));
            Logger.Information("**************************************************************");

            return true;
        }
    }
}
