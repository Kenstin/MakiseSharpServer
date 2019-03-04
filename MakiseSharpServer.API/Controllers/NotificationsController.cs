using System;
using System.Linq;
using System.Threading.Tasks;
using MakiseSharpServer.API.Results;
using MakiseSharpServer.Application.Notification.Commands.CreateNotification;
using MakiseSharpServer.Application.Notification.DTOs;
using MakiseSharpServer.Application.Notification.Errors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MakiseSharpServer.API.Controllers
{
    public class NotificationsController : BaseController
    {
        /// <summary>
        /// Creates a new notification
        /// </summary>
        /// <param name="command">Post data</param>
        /// <returns>Created notification</returns>
        [HttpPost]
        [SwaggerResponse(400, "Requested status provider is not supported.")]
        [SwaggerResponse(400, "Wrong Discord OAuth2 code provided.")]
        public async Task<ActionResult<NotificationCreatedDto>> CreateNotificationAsync([FromBody] CreateNotificationCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Errors.Any(e => e is StatusProviderNotSupportedError))
            {
                return new MessageResult(400, "Requested status provider is not supported.");
            }

            if (result.Errors.Any(e => e is WrongCodeError))
            {
                return new MessageResult(400, "Wrong Discord OAuth2 code provided.");
            }

            throw new NotImplementedException();
        }
    }
}
