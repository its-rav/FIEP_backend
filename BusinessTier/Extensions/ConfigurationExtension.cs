
using Autofac;
using BusinessTier.DistributedCache;
using DataTier.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTier.Extensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddMonitoringServicesDBConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<FIEPContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnectionString"));
                options.UseLazyLoadingProxies();
            });
            return serviceCollection;
        }
        public static void UseCaching<T>(this ContainerBuilder builder)
            where T : ICacheStore
        {
            builder.RegisterType<T>().AsSelf().As<ICacheStore>();
        }
    }
}
