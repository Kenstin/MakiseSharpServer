using System;
using MakiseSharpServer.Models.Settings;
using MakiseSharpServer.Services;
using MakiseSharpServer.Services.APIs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace MakiseSharpServer
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
            settings.Discord.RedirecUri = new Uri(Configuration["discord:redirectUri"]);
            settings.Discord.ApiUri = new Uri(Configuration["discord:apiUri"]);
            //Configuration.Get doesn't automatically map Uri
            services.AddSingleton(settings);

            services.AddRefitClient<IDiscordApi>()
                .ConfigureHttpClient(c => c.BaseAddress = settings.Discord.ApiUri);
            services.AddScoped<IDiscordJwtCreator, JwtCreator>();
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
