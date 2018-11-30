using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApp.DI
{
    interface IKV<K, V>
    {
        K Key { get; }
        V GetValue(IServiceProvider sp);
    }

    class KV_2<K, V> : IKV<K, V>
    {
        Func<IServiceProvider, K, V> factory;

        public KV_2(K key, Func<IServiceProvider, K, V> factory)
        {
            Key = key;
            this.factory = factory;
        }

        public K Key { get; }

        public V GetValue(IServiceProvider sp) => factory(sp, Key);
    }

    class KV_0<K, V> : IKV<K, V>
    {
        Lazy<V> lazy;

        public KV_0(IServiceProvider sp, K key, Func<IServiceProvider, K, V> factory)
        {
            Key = key;
            lazy = new Lazy<V>(() => factory(sp, Key), true);
        }

        public K Key { get; }

        public V GetValue(IServiceProvider sp) => lazy.Value;
    }

    public static class KV_Ext
    {
        public static IServiceCollection AddTransientWithKey<K, V>(this IServiceCollection services, K key, Func<IServiceProvider, K, V> factory)
        {
            return services.AddScoped<IKV<K, V>>((sp) => new KV_2<K, V>(key, factory));
        }

        public static IServiceCollection AddSingletonWithKey<K, V>(this IServiceCollection services, K key, Func<IServiceProvider, K, V> factory)
        {
            return services.AddSingleton<IKV<K, V>>((sp) => new KV_0<K, V>(sp, key, factory));
        }

        public static IServiceCollection AddScopedWithKey<K, V>(this IServiceCollection services, K key, Func<IServiceProvider, K, V> factory)
        {
            return services.AddScoped<IKV<K, V>>((sp) => new KV_0<K, V>(sp, key, factory));
        }

        public static IServiceCollection AddTransientWithKey<K, TService, TImplementation>(this IServiceCollection services, K key)
            where TService : class
            where TImplementation : class, TService
        {
            services.TryAddTransient<TImplementation>();
            return services.AddTransientWithKey<K, TService>(key, (sp, k) => sp.GetService<TImplementation>());
        }

        public static IServiceCollection AddSingletonWithKey<K, TService, TImplementation>(this IServiceCollection services, K key)
            where TService : class
            where TImplementation : class, TService
        {
            services.TryAddTransient<TImplementation>();
            return services.AddSingletonWithKey<K, TService>(key, (sp, k) => sp.GetService<TImplementation>());
        }

        public static IServiceCollection AddScopedWithKey<K, TService, TImplementation>(this IServiceCollection services, K key)
            where TService : class
            where TImplementation : class, TService
        {
            services.TryAddTransient<TImplementation>();
            return services.AddScopedWithKey<K, TService>(key, (sp, k) => sp.GetService<TImplementation>());
        }

        public static IServiceCollection AddTransientWithKey<K, TService>(this IServiceCollection services, K key) where TService : class
            => AddTransientWithKey<K, TService, TService>(services, key);

        public static IServiceCollection AddSingletonWithKey<K, TService>(this IServiceCollection services, K key) where TService : class
            => AddSingletonWithKey<K, TService, TService>(services, key);

        public static IServiceCollection AddScopedWithKey<K, TService>(this IServiceCollection services, K key) where TService : class
            => AddScopedWithKey<K, TService, TService>(services, key);

        public static IServiceCollection AddTransientWithKey<TService, TImplementation>(this IServiceCollection services, string key)
            where TService : class
            where TImplementation : class, TService
            => AddTransientWithKey<string, TService, TImplementation>(services, key);

        public static IServiceCollection AddSingletonWithKey<TService, TImplementation>(this IServiceCollection services, string key)
            where TService : class
            where TImplementation : class, TService
            => AddSingletonWithKey<string, TService, TImplementation>(services, key);

        public static IServiceCollection AddScopedWithKey<TService, TImplementation>(this IServiceCollection services, string key)
            where TService : class
            where TImplementation : class, TService
            => AddScopedWithKey<string, TService, TImplementation>(services, key);

        public static IServiceCollection AddTransientWithKey<TService>(this IServiceCollection services, string key) where TService : class
            => AddTransientWithKey<string, TService, TService>(services, key);

        public static IServiceCollection AddSingletonWithKey<TService>(this IServiceCollection services, string key) where TService : class
            => AddSingletonWithKey<string, TService, TService>(services, key);

        public static IServiceCollection AddScopedWithKey<TService>(this IServiceCollection services, string key) where TService : class
            => AddScopedWithKey<string, TService, TService>(services, key);

        public static IServiceCollection AddTransientWithKey<V>(this IServiceCollection services, string key, Func<IServiceProvider, string, V> factory) => AddTransientWithKey<string, V>(services, key, factory);
        public static IServiceCollection AddSingletonWithKey<V>(this IServiceCollection services, string key, Func<IServiceProvider, string, V> factory) => AddSingletonWithKey<string, V>(services, key, factory);
        public static IServiceCollection AddScopedWithKey<V>(this IServiceCollection services, string key, Func<IServiceProvider, string, V> factory) => AddScopedWithKey<string, V>(services, key, factory);

        public static V GetService<K, V>(this IServiceProvider serviceProvider, K key)
        {
            var kv = serviceProvider.GetServices<IKV<K, V>>().SingleOrDefault(_ => _.Key.Equals(key));
            return kv != null ? kv.GetValue(serviceProvider) : default;
        }

        public static IEnumerable<V> GetServices<K, V>(this IServiceProvider serviceProvider, K key)
        {
            return serviceProvider.GetServices<IKV<K, V>>().Where(_ => _.Key.Equals(key)).Select(_ => _.GetValue(serviceProvider));
        }

        public static T GetService<T>(this IServiceProvider serviceProvider, string key) => GetService<string, T>(serviceProvider, key);
        public static IEnumerable<T> GetServices<T>(this IServiceProvider serviceProvider, string key) => GetServices<string, T>(serviceProvider, key);
    }
}