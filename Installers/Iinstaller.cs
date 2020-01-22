using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Installers
{
    public interface Iinstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
