using System;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public class Notification : Entity, IAggregateRoot
    {
        public Webhook Webhook { get; private set; }

        public Repository Repository { get; private set; }

        public Guid AuthorId { get; private set; }

        /// <summary>
        /// Gets or sets the last build number for which a notification has been sent
        /// </summary>
        public uint LastBuildNumber { get; set; }
    }
}
