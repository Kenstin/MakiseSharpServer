using System.Collections.Generic;
using System.Linq;
using MakiseSharpServer.Application.Notification.Commands.CreateNotification;

namespace MakiseSharpServer.Application.Notification.Models.StatusProviders
{
    public class StatusProviderFactory : IStatusProviderFactory
    {
        private readonly IEnumerable<IStatusProvider> statusProviders;

        public StatusProviderFactory(IEnumerable<IStatusProvider> statusProviders)
        {
            this.statusProviders = statusProviders;
        }

        public IStatusProvider GetStatusProviderForCommand(CreateNotificationCommand command) => statusProviders.FirstOrDefault(prov => prov.DoesSupport(command));
    }
}
