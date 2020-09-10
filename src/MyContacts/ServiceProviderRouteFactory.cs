using Microsoft.Extensions.DependencyInjection;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyContacts
{
    public static class ServiceProviderRouteFactoryExtensions
    {
        public static IServiceCollection RegisterRoute(this IServiceCollection services, Type type)
        {
            services.AddTransient(type);
            ServiceProviderRouteFactory.RegisterRoute(type);
            return services;
        }

        public static IServiceCollection RegisterRoute(this IServiceCollection services, Type type, string routename)
        {
            services.AddTransient(type);
            ServiceProviderRouteFactory.RegisterRoute(type, routename);
            return services;
        }
    }

    public class ServiceProviderRouteFactory : RouteFactory
    {
        public static void RegisterRoute(Type type)
        {
            RegisterRoute(type, type.Name);
        }

        public static void RegisterRoute(Type type, string routename)
        {
            Routing.RegisterRoute(routename, new ServiceProviderRouteFactory(type));
        }

        readonly Type type;

        public ServiceProviderRouteFactory(Type type)
        {
            this.type = type;
        }

        public override Element GetOrCreate()
        {
            return (Element)MyContacts.App.Current.Services.GetService(type);
        }

        public override bool Equals(object obj)
        {
            if ((obj is ServiceProviderRouteFactory typeRouteFactory))
                return typeRouteFactory.type == type;

            return false;
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }
    }
}
