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
using Microsoft.OpenApi.Models;
using System.IO;
using Newtonsoft.Json;
using BusinessTier.DistributedCache;
using BusinessTier.Services;
using FIEP_API.Middlewares;

namespace FIEP_API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //cors
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            //sql connection
            services.AddMonitoringServicesDBConfiguration(Configuration);
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration.GetConnectionString("RedisCacheConnection");
            });

            services.AddTransient<NotificationPublisher>();

            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
            
            // services.AddApplicationInsightsTelemetry();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            
            //regist handlers
            services.RegisterHandlers();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<RedisCacheService>().As<IRedisCacheService>();
            builder.UseCaching<ApplyCache>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string
                c.RoutePrefix = string.Empty;
            });

            //middleware
            //app.UseAuthorizationMiddleware();
            app.UseUpdateRedisAndSheetMiddleware();

            var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "keys", "firebase-fiep-private-key.json");
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(pathToKey)
            });
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.GetApplicationDefault(),
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
