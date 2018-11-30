using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

//[assembly: zhen_di_shi_ri_le_gou_le.DIRegister(typeof(IList<>), Key = "1", Lifetime = ServiceLifetime.Transient)]

namespace zhen_di_shi_ri_le_gou_le
{
    public/*internal*/ interface IDepInject
    {
        //#rg//public {className}(IServiceProvider serviceProvider) { }

        //#[med]//void zhen_di_shi_ri_le_gou_le__ctor_init_{xxx}();
    }

    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public/*internal*/ sealed class DInject : Attribute
    {
        public DInject() { }
        public DInject(string key) { } //object??
    }

    [Conditional("DEBUG")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public/*internal*/ sealed class DIRegister : Attribute
    {
        public DIRegister(Type serviceType) { }
        public DIRegister(Type serviceType, Type implementationType) { }

        public string Key { get; set; } //object??
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
    }
}