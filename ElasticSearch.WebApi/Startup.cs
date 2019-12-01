using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NascorpLib.Cache.Redis;
using NascorpLib.WebSocketManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ElasticSearch.ApplicationService;
using ElasticSearch.ApplicationService.AuthenticationService;
using ElasticSearch.ApplicationService.LoginService;
using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.IRepository;
using ElasticSearch.Domain.Utilities;
using ElasticSearch.Repository;
using ElasticSearch.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using Nest;
using ElasticSearch.WebApi.Utilities;

namespace ElasticSearch.WebApi
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
            var key = Environment.GetEnvironmentVariable("JWT_KEY");

            services.AddMvc()
                   .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                   .AddJsonOptions(o => o.SerializerSettings.Formatting = Formatting.None);

            services.AddElasticsearch(Configuration);

            // Add framework services.
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            //enable manager websocket
            services.AddWebSocketManager();

            //ENABLE SERVICES AWS
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "APICatalog - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API.",
                    TermsOfService = "Terms Of Service",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Claudinei Nascimento",
                        Email = "claudinei@nascorp.com.br"
                    },
                    License = new Swashbuckle.AspNetCore.Swagger.License
                    {
                        Name = "Apache 2.0",
                        Url = "http://www.apache.org/licenses/LICENSE-2.0.html"
                    }
                });
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddFilter<ConsoleLoggerProvider>
                    ((category, level) => category == "A" || level == Microsoft.Extensions.Logging.LogLevel.Critical);
            });

            services.AddDbContext<ElasticSearchContext>(option => option.UseMySql(Config.ConnectionString, mySqlOptions =>
            {
                mySqlOptions.ServerVersion(new Version(5, 7, 17), ServerType.MySql); // replace with your Server Version and Type
            }));

            services.AddResponseCaching();
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = new List<string> { "application/json" };
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddScoped<DbContext>(sp => sp.GetService<ElasticSearchContext>());
            services.AddSingleton<CacheExchange>(provider => new CacheExchange(Config.CacheRedisConnection, Config.CacheRedisDatabase, Config.CacheInstanceName, Config.CacheExpireTime, Config.CacheRedisTimeout));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IJwtTokenApplication, JwtTokenApplication>();
            services.AddTransient<ILoginApplication, LoginApplication>();


            services.AddTransient<IMeuServicoApplicationService, MeuServicoApplicationService>();
            services.AddTransient<IMeuServicoRepository, MeuServicoRepository>();

            //Setting up Jwt Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            var defaultDateCulture = "pt-BR";
            var ci = new CultureInfo(defaultDateCulture);
            ci.NumberFormat.NumberDecimalSeparator = ",";
            ci.NumberFormat.CurrencyDecimalSeparator = ",";

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo> { ci },
                SupportedUICultures = new List<CultureInfo> { ci }
            });

            app.UseCors("AllowAll");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
            app.UseDeveloperExceptionPage();

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
            });

            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromHours(23)
                    };
                context.Response.Headers[HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseStaticFiles();
        }

      
    }
}
