using MyS.Service3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class MyS_Service3_dodo_Service1 : I_Am_A_Service_Controller
{
    public void ATest(ActionContext actionContext) { }
}

namespace MyS.Service3.dodo
{
    public partial class empty_Controller : I_Am_A_Service_Controller
    { }

    public partial class AService : I_Am_A_Service_Controller
    {
        public void ATest(
            [BP(nameof(MyS.Service3.dodo.Filters.B1), 3)]
            string s
        )
        { }
    }

    public class Not_A_partial_Class
    {
        public class Not_A_partial_Class1
        {
            public partial class WService : AService
            {
                public WService(ActionContext context) { }

                public void ATest(string actionName) { }

                public async Task Test()
                {
                    await Task.CompletedTask;
                }
            }
        }
    }

    public partial class BackgroundService : I_Am_A_Service_Controller
    {
        public ActionContext ActionContext { get; set; }

        [OnFilter(nameof(Filters.F1), 100, Order = 1)]
        [OnFilter(nameof(Filters), 200, Order = 2)]
        public async ValueTask<long> Test(
            ActionContext actionContext,

            [BP(nameof(fn_ps_Test2_1), true)]
            int i = 1
        )
        {
            await Task.CompletedTask;
            return 100;
        }

        public async Task<IActionResult> Test1(string actionName)
        {
            await Task.CompletedTask;
            return new EmptyResult();
        }
    }
}