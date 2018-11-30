using Microsoft.Extensions.DependencyInjection;
using MyWebApp.DI;
using MyWebApp.IServices;
using System;
using System.Collections.Generic;

namespace MyWebApp.Jobs
{
    public static class DI_ConfigureServices
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            #region zhen_di_shi_ri_le_gou_le.DIRegister services
            ////rg@2018/11/30 15:57:47
            services.AddScoped(typeof(MyWebApp.Jobs.SimpleJob2));
            #endregion zhen_di_shi_ri_le_gou_le.DIRegister services
        }
    }
}
