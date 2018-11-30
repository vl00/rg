using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyS.Service3.dodo
{
    public class Filters
    {
        public async Task<IActionResult> F1(ActionContext actionContext, Func<Task<IActionResult>> next, double d)
        {
            return await next();
        }

        public Task<string> B1(ActionContext actionContext, (string Name, bool HasDefv, string Defv) p, int b)
        {
            return Task.FromResult("sss");
        }
    }

    public class Filters2
    {
        public Task<string> B1(ActionContext actionContext, (string Name, bool HasDefv, string Defv) p, int b)
        {
            return Task.FromResult("sss");
        }
    }

    [OnFilter(nameof(Filters.F1))]
    public partial class BackgroundService //: I_Am_A_Service_Controller
    {
        int fn_ps_Test2_1(ActionContext actionContext, (string Name, bool HasDefv, object Defv) p, bool b)
        {
            return -11;
        }

        public IActionResult Test2(
            [BP(nameof(Filters.B1), 3)]
            string s1,

            [BP(nameof(MyS.Service3.dodo.Filters2.B1), 3)]
            string s2,

            [BP(nameof(fn_ps_Test2_1), true)]
            int i = 1
        )
        {
            return new ObjectResult(++i);
        }
    }
}