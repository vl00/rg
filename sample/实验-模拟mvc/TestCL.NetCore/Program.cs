using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Program
{
    public static void Main(string[] args)
    {
        async_Main(args).GetAwaiter().GetResult();
    }

    static async Task async_Main(string[] args)
    {
        await Task.CompletedTask;

        await MyS.Service3.ActionContext.Execute(new MyS.Service3.ActionContext
        {
			ControllerName = "BackgroundService",
			ActionName = "Test",
        });
    }
}