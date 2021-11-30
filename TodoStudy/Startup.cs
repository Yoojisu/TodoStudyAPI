using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpsPolicy;
using TodoStudy.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TodoStudy.Exceptions;
using System.Net;
using System.Reflection;
using System.Text;
using NJsonSchema;

namespace TodoStudy
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
            services
                .AddAuthentication(
                //CookieAuthenticationDefaults.AuthenticationScheme
                options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }

                )
                .AddCookie(options =>
                {
                    //options.LoginPath = "/api/auth";
                    options.Cookie.HttpOnly = false;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

   
            services.AddCors(options =>
            {
                var origins = Configuration.GetSection("AllowedOrigins")
                    .GetChildren()
                    .Select(a => a.Value)
                    .ToArray();

                var builder = new CorsPolicyBuilder(origins);
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();

                options.AddDefaultPolicy(builder.Build());
            });

            //DBConnection
            var database = Configuration.GetSection("Database");
            var connectionString = database["ConnectionString"];
            services.AddDbContext<TodoDBContext>(options =>
            {
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                //ptions.UseMySql(connectionString);
                options.UseLazyLoadingProxies().UseMySql(connectionString);

            });

            //swagger

            services.AddOpenApiDocument(options =>
            {
                options.PostProcess = document =>
                {
                    document.Info.Title = "Jisu's Todo Api ";
                    document.Info.Description = "A simple ASP.NET Core web API";
                };
            });

            //Router
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseExceptionHandler(new ExceptionHandlerOptions() { ExceptionHandler = ExceptionHandler });

            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseAuthentication();

            //// CORS
            //app.UseCors();

            //// Swagger
            //app.UseOpenApi();
            //app.UseSwaggerUi3();

            //app.UseMvc();

            app.UseExceptionHandler(new ExceptionHandlerOptions() { ExceptionHandler = ExceptionHandler });
            // Swagger
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseCookiePolicy();
            app.UseAuthentication();
            // CORS
            app.UseCors();
            app.UseMvc();

           // app.UseStaticFiles();
            
        }

        public async Task ExceptionHandler(HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = "알 수 없는 에러가 발생하였습니다.";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                if (contextFeature.Error is HttpException httpException)
                {
                    code = httpException.Code;
                    message = httpException.GetResponseMessage();
                }
                else if (contextFeature.Error is Exception exception)
                {
                    message = exception.ToString();
                }
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(message, Encoding.UTF8);
        }

    }
}
