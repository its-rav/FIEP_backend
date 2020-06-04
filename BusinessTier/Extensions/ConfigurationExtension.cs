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
            serviceCollection.AddDbContext<FEIP_be_dbContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DatabaseConnectionString")));
            return serviceCollection;
        }
    }
}
