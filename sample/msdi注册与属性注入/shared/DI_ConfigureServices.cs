/// use in dlls with `${dll_file_name}.DI_ConfigureServices`

/*//eg:
  
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace {dll_name} //eg: MyWebApp.Jobs
{
    public static class DI_ConfigureServices
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        { 
			//...
			
			#region zhen_di_shi_ri_le_gou_le.DIRegister services
			//..
			#endregion zhen_di_shi_ri_le_gou_le.DIRegister services
			
			//...
        }
    }
}

//*/