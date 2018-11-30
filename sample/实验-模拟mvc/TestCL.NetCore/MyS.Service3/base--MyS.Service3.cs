using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyS.Service3
{
    public partial class ActionContext
    {
        public string ControllerName { get; }
        public string ActionName { get; }
        //#props//

        //#impl// public ActionContext(string controllerName, string actionName, ...);

        public Task ExecuteAsync(IActionResult actionResult) => actionResult.ExecuteResultAsync(this);

        //#impl// public static Task Execute(ActionContext actionContext);

        //#rg// public static T __ct__<T>(object o);
        //#rg// public static Func<Task<IActionResult>> __callca__(params Func<Func<Task<IActionResult>>, Func<Task<IActionResult>>>[] fs);
        //#rg// public static T __defaultBindp__<T>(Exception ex);
        //#rg// public static Task<T> __defaultBindp__<T>(ActionContext ctx, string pname, bool phasDefv, T pdefv);
        //#rg// public static Task<T1> __defaultBindp__<T1, T2>(ActionContext ctx, string pname, bool phasDefv, T1 pdefv, Func<IEnumerable<T2>, T1> cf);
    }

    public partial class ActionContext
    {
        public static readonly HandlersList<Func<ActionContext, Task<IActionResult>>> Handlers = new HandlersList<Func<ActionContext, Task<IActionResult>>>(new object());

        //#impl//
        public static async Task Execute(ActionContext actionContext)
        {
            var fhs = Handlers.GetList();
            if (fhs == null) throw new InvalidOperationException();
            foreach (var f in fhs)
            {
                var t = f(actionContext);
                if (t == null) continue;
                var r = await t;
                if (r != null)
                    await r.ExecuteResultAsync(actionContext);
                break;
            }
        }

        public struct HandlersList<T>
        {
            IList<T> c, n;
            readonly object o;

            public HandlersList(object o)
            {
                this.o = o;
                c = n = null;
            }

            IList<T> getn()
            {
                lock (o)
                {
                    if (n == c)
                    {
                        n = c?.ToList() ?? new List<T>();
                        c = null;
                    }
                    return n;
                }
            }

            IList<T> getc()
            {
                lock (o)
                {
                    return c = n;
                }
            }

            public T Add(T listener)
            {
                getn().Add(listener);
                return listener;
            }

            public void Remove(T listener)
            {
                getn().Remove(listener);
            }

            public IList<T> GetList()
            {
                return getc();
            }
        }
    }

    public interface IActionResult
    {
        Task ExecuteResultAsync(ActionContext actionContext);
    }

    public interface I_Am_A_Service_Controller
    {
        /* //rg impl
        internal (new) static class __rg__
        {
            static {c} get_ctrl(ActionContext actionContext);
            public static Task<IActionResult> HandleAsync(ActionContext actionContext);
            public static Task<IActionResult> HandleActionAsync(ActionContext actionContext, Func<ActionContext, {c}> _get_this_);
        }
        */
    }

    //#using//
    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    //public sealed class I_Am_A_Service_Action : Attribute
    //{
    //    public I_Am_A_Service_Action() { }
    //    public I_Am_A_Service_Action(string action) { }
    //}
}

namespace MyS.Service3
{
    //#impl// public class ErrorResult : IActionResult
    //#impl// public class ObjectResult : IActionResult

    public class EmptyResult : IActionResult
    {
        public Task ExecuteResultAsync(ActionContext actionContext) => Task.CompletedTask;
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class BP : Attribute
    {
        public BP(string nameofMethod, params object[] pargs) { }
    }

    public interface IBindP
    {
        //struct BindpCtx<T>(string Name, bool HasDefv, T Defv) //BindpCtx.New<T>(string Name, bool HasDefv, T Defv);
        Task<T> BindP<T>(ActionContext actionContext, (string Name, bool HasDefv, T Defv) p);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class OnFilter : Attribute
    {
        public OnFilter(string nameofMethod, params object[] pargs) { }

        public int Order { get; set; }
    }

    public interface IFilter
    {
        int Order { set; }
        Task<IActionResult> OnInvoke(ActionContext actionContext, Func<Task<IActionResult>> next);
    }
}
