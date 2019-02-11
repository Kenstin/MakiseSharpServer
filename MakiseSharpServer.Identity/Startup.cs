using System;
using System.Linq;
using System.Reflection;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using MakiseSharpServer.Persistence;
using MakiseSharpServer.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MakiseSharpServer.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Settings
            var settings = Configuration.Get<AppSettings>();
            settings.Discord.ApiUri = new Uri(Configuration["discord:apiUri"]);
            settings.Discord.CdnUri = new Uri(Configuration["discord:cdnUri"]);
            services.AddSingleton(settings);

            //IdentityServer
            var builder = services.AddIdentityServer();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddEntityFrameworkSqlite();
            
            if (Environment.IsDevelopment())
            {
                builder.AddInMemoryIdentityResources(DevConfig.GetIdentityResources())
                    .AddInMemoryApiResources(DevConfig.GetApis())
                    .AddInMemoryClients(DevConfig.GetClients())
                    .AddDeveloperSigningCredential();
            }
            else
            {
                builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlite(Configuration["database:identityServerConnectionString"], sql =>
                            sql.MigrationsAssembly(migrationsAssembly));
                }).AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlite(Configuration["database:identityServerConnectionString"], sql =>
                            sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                });

                throw new Exception("need to configure key material");
            }
 
            //UserRepository
            services.AddDbContext<MakiseDbContext>(options =>
            {
                options.UseSqlite(Configuration["database:connectionString"], o =>
                {
                    o.MigrationsAssembly(typeof(UserRepository).Assembly.GetName().Name);
                });
            });
            services.AddTransient<IUserRepository, UserRepository>();

            //Discord auth
            services.AddAuthentication()
                .AddDiscord(c =>
                {
                    c.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    c.AppId = settings.Discord.ClientId.ToString();
                    c.AppSecret = settings.Discord.ClientSecret;
                    c.SaveTokens = true;
                });

            //Antiforgery
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "anti-forgery";
                options.HeaderName = "XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });

            //MVC
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(SecurityHeadersAttribute));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                //Reverse proxy
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
