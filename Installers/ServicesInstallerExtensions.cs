using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Installers
{
    public static class ServicesInstallerExtensions
    {
        public static void InstallAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            var installerServicesInstances = typeof(Startup).Assembly.ExportedTypes.Where(x => typeof(Iinstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                                                                   .Select(Activator.CreateInstance).Cast<Iinstaller>().ToList();
            installerServicesInstances.ForEach(x => x.InstallServices(services, configuration));
        }
    }
}
