
using Common;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.DI;
using System;
using System.Collections.Generic; 

namespace MyWebApp.Jobs
{ 
	partial class SimpleJob2 
	{
	    public SimpleJob2(System.IServiceProvider di) 
	    { 
	        this._serviceProvider = di; 
	    }
	
	    static string __rg_di_key_error__(System.Exception ex) => throw ex;
	} 
	

}