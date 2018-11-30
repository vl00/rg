using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace MyWebApp
{
    public partial class Program
    {
        static Assembly[] _myAssemblies;

        static Assembly[] Get_MyWebApp_Assemblies()
        {
            return _myAssemblies ?? (_myAssemblies = __(AssemblyLoadContext.Default).ToArray());

            IEnumerable<Assembly> __(AssemblyLoadContext assemblyLoadContext)
            {
                //var dlls = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"{nameof(MyWebApp)}*.dll");
                var dlls = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*.*.dll");
                Assembly a = null;

                foreach (var dll in dlls)
                {
                    try
                    {
                        a = assemblyLoadContext.LoadFromAssemblyPath(dll);
                    }
                    catch
                    {
                        continue;
                    }

                    //var deps = DependencyContext.Default.Merge(DependencyContext.Load(a));
                    ////Microsoft.Extensions.DependencyModel.DependencyContextLoader
                    //var cls = deps.CompileLibraries.OrderBy(_ => _.Name); //.Where(d => !d.Name.StartsWith($"{nameof(MyWebApp)}."));
                    //var rs = deps.RuntimeLibraries.OrderBy(_ => _.Name);

                    //foreach (var cl in cls)
                    //{
                    //    assemblyLoadContext.LoadFromAssemblyName(new AssemblyName(cl.Name));
                    //}

                    yield return a;
                }
            }
        }

        void configure_external_Services(IServiceCollection services)
        {
            ///`${dll_file_name}.DI_ConfigureServices`

            foreach (var a in Get_MyWebApp_Assemblies())
            {
                //if (a == Assembly.GetEntryAssembly()) continue; //当前assembly

                var dity = a.GetType($"{a.GetName().Name}.DI_ConfigureServices", false);
                if (dity == null) continue;

                var mi = dity.GetRuntimeMethods().FirstOrDefault(_ => _.Name == "ConfigureServices");
                if (mi == null) continue;

                var pis = mi.GetParameters();
                var o = new object[pis.Length];

                for (var i = 0; i < o.Length; i++)
                {
                    var pi = pis[i];

                    if (pi.ParameterType == typeof(IServiceCollection))
                        o[i] = services;
                    //else if (pi.ParameterType == typeof(IServiceProvider))
                    //    o[i] = GlobalServiceProvider;
                    else if (pi.ParameterType == typeof(IConfiguration))
                        o[i] = Configuration;
                    else
                        throw new NotSupportedException();
                }

                mi.Invoke(null, o);
            }
        }
    }
}
