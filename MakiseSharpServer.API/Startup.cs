using System;
using MakiseSharpServer.Application.ApiClients.DiscordApi;
using MakiseSharpServer.Application.Authentication.Commands.CreateToken;
using MakiseSharpServer.Application.Authentication.Services;
using MakiseSharpServer.Application.Settings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var settings = Configuration.Get<AppSettings>();
            settings.Discord.RedirectUri = new Uri(Configuration["discord:redirectUri"]);
            //Configuration.Get doesn't automatically map Uri
            services.AddSingleton(settings);
            services.AddScoped<IDiscordJwtCreator, JwtCreator>();
            services.AddScoped<ITokenFactory, TokenFactory>();

            services.AddMediatR(typeof(CreateTokenCommandHandler));
            services.AddRefitClient<IDiscordApi>()
                .ConfigureHttpClient(c => c.BaseAddress = settings.Discord.ApiUri);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
