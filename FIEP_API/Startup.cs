using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Autofac;
using DataTier.Repository;
using DataTier.UOW;
using BusinessTier.Extensions;

namespace FIEP_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //sql connection
            services.AddMonitoringServicesDBConfiguration(Configuration);

            services.AddControllers();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["ConnectionStrings:CacheConnection"];
            });
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["ConnectionStrings:CacheConnection"];
            });
            // services.AddApplicationInsightsTelemetry();

            
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
