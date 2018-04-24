using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class DIExtensions
    {
        public static IEnumerable<Assembly> GetAssembliesInDirectory(string directory = "", Func<FileInfo, bool> selector = null)
        {
            if (selector == null)
                selector = (_) => true;

            var target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);

            var files = new DirectoryInfo(target).GetFiles();
            var assemblies =
                files.Where(x => x.Extension.ToLower() == ".dll").Where(selector)
                    .Select(x =>
                    {
                        try
                        {
                            return Assembly.Load(AssemblyName.GetAssemblyName(x.FullName));
                        }
                        catch (FileNotFoundException) { }
                        return null;
                    });
            return assemblies.Where(x => x != null).ToList();
        }

    }
}
