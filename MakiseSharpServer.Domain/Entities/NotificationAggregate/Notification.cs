using System;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public class Notification : Entity, IAggregateRoot
    {
        public Notification(Webhook webhook, Repository repository, Guid authorId)
        {
            Webhook = webhook ?? throw new ArgumentNullException(nameof(webhook));
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            AuthorId = authorId;
        }

        public Webhook Webhook { get; private set; }

        public Repository Repository { get; private set; }

        public Guid AuthorId { get; private set; }

        /// <summary>
        /// Gets the last build number for which a notification has been sent
        /// </summary>
        public uint LastBuildNumber { get; private set; }
    }
}
