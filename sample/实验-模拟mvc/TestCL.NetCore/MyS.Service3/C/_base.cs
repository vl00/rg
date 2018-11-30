using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyS.Service3
{
    public partial class ActionContext
    {
        //public object DI { get; }
        //public dynamic JsonData { get; }

        //#rg// 
        public static Func<Task<IActionResult>> __callca__(params Func<Func<Task<IActionResult>>, Func<Task<IActionResult>>>[] fs)
        {
            Func<Task<IActionResult>> d = null;
            for (var i = fs.Length - 1; i >= 0; i--)
                d = fs[i]?.Invoke(d) ?? d;
            return d;
        }

        //#rg// 
        public static T __ct__<T>(object o)
        {
            if (o is T t) return t;
            //if (o is T) return (T)o;
            return (T)Convert.ChangeType(o, typeof(T));
        }

        //#rg//
        public static T __defaultBindp__<T>(Exception ex) => throw ex;
        public static async Task<T> __defaultBindp__<T>(ActionContext ctx, string pname, bool phasDefv, T pdefv)
        {
            await Task.CompletedTask;
            //#impl//...

            if (phasDefv) return pdefv;
            throw new ArgumentException(pname);
        }
        public static async Task<T1> __defaultBindp__<T1, T2>(ActionContext ctx, string pname, bool phasDefv, T1 pdefv, Func<IEnumerable<T2>, T1> cf)
               where T1 : IEnumerable<T2>
        {
            await Task.CompletedTask;
            /*//#impl//...
            T1 to_t(StringValues svs) => cf(svs.ToArray().Select(s => (T2)Convert.ChangeType(s, typeof(T2))));

            try
            {
                {
                    if (context.Request.HasFormContentType)
                    {
                        var form = await context.Request.ReadFormAsync();
                        var vs = form[pname];
                        if (vs.Count > 0) return to_t(vs);
                    }
                }
                {
                    //if (routeData.Values.TryGetValue(pname, out var v))
                    //{
                    //    return (T)Convert.ChangeType(v as string ?? v?.ToString() ?? string.Empty, typeof(T));
                    //}
                }
                {
                    var vs = context.Request.Query[pname];
                    if (vs.Count > 0) return to_t(vs);
                }
            }
            catch { } /*///

            if (phasDefv) return pdefv;
            throw new ArgumentException(pname);
        }
    }

    //#impl//
    public class ErrorResult : IActionResult
    {
        public ErrorResult(Exception ex) => Error = ex;

        public Exception Error { get; }

        public Task ExecuteResultAsync(ActionContext actionContext)
        {
            throw Error;
        }
    }

    //#impl//
    public class ObjectResult : IActionResult
    {
        readonly object obj;

        public ObjectResult(object obj) => this.obj = obj;

        public Task ExecuteResultAsync(ActionContext actionContext)
        {
            //...
            throw new NotSupportedException();
        }
    }
}
