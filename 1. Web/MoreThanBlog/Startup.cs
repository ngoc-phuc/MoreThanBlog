using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MoreThanBlog.Setup;
using Newtonsoft.Json.Converters;

namespace MoreThanBlog
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
            CoreServiceInstaller.ConfigureCoreServices(services, Configuration);
            ApplicationServicesInstaller.ConfigureApplicationServices(services, Configuration);
            AuthServicesInstaller.ConfigureServicesAuth(services, Configuration);

            //ignore handle loop json
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            //add swagger
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc(
                        "v5",
                        new OpenApiInfo
                        {
                            Title = "More Than Blog API",
                            Version = "v5",
                            Description = "ASP.NET Core Web API",
                            Contact = new OpenApiContact
                            {
                                Name = "More than team",
                                Email = "morethanteam@gmail.com",
                            },
                            License = new OpenApiLicense
                            {

                                Name = "Copyright by More Than Team",
                            }
                        });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });

                    c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.DocumentTitle = "More Than Document";
                c.SwaggerEndpoint("v5/swagger.json", "My API V5");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}