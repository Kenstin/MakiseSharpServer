using System;
using System.IO;
using System.Reflection;
using MakiseSharpServer.API.Swagger.Examples;
using MakiseSharpServer.API.Swagger.Filters;
using MakiseSharpServer.Application.ApiClients.DiscordApi;
using MakiseSharpServer.Application.Authentication.Commands.CreateToken;
using MakiseSharpServer.Application.Authentication.Services;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using MakiseSharpServer.Persistence;
using MakiseSharpServer.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace MakiseSharpServer.API
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
            services.AddMvcCore(o =>
            {
                o.ReturnHttpNotAcceptable = false;

            }).AddJsonFormatters()
                .AddApiExplorer()
                .AddDataAnnotations()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MakiseSharpServer API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
                c.ExampleFilters();

                c.OperationFilter<JsonResponseOperationFilter>();
                c.OperationFilter<MessageResultExampleOperationFilter>();
                c.DocumentFilter<MessageResultSchemaOverrideDocumentFilter>();
                //NOTE: using c.MapType<MessageResult> didn't seem to affect definition
            });

            var settings = Configuration.Get<AppSettings>();
            services.AddSingleton(settings);

            services.AddEntityFrameworkSqlite().AddDbContext<MakiseDbContext>(options =>
            {
                options.UseSqlite(Configuration["database:connectionString"], o =>
                {
                    o.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name);
                });
            });

            services.AddScoped<IDiscordJwtCreator, JwtCreator>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMediatR(typeof(CreateTokenCommandHandler));
            services.AddRefitClient<IDiscordApi>()
                .ConfigureHttpClient(c => c.BaseAddress = settings.Discord.ApiUri);
            services.AddSwaggerExamplesFromAssemblyOf<JwtResponseExample>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwaggerUI(c => //swaggerUI only on dev, using independently hosted ReDoc on production
                {
                    c.SwaggerEndpoint("/api/v1/swagger.json", "MakiseSharpServer");
                });
            }

            app.UseSwagger(c => c.RouteTemplate = "api/{documentName}/swagger.json");
            app.UseMvc();
        }
    }
}
