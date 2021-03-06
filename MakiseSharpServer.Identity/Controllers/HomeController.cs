﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Identity.Models;

namespace MakiseSharpServer.Identity.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IHostingEnvironment environment;
        private readonly AppSettings appSettings;

        public HomeController(IIdentityServerInteractionService interaction, IHostingEnvironment environment, AppSettings appSettings)
        {
            this.interaction = interaction;
            this.environment = environment;
            this.appSettings = appSettings;
        }

        public IActionResult Index()
        {
            return Redirect(appSettings.Site.IndexRedirectTo.ToString());
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }
    }
}
