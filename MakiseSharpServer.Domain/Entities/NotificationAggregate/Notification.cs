using System;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public class Notification : Entity, IAggregateRoot
    {
        public Notification(Webhook webhook, string statusProvider, string slug, Guid authorId)
        {
            Webhook = webhook ?? throw new ArgumentNullException(nameof(webhook));
            StatusProvider = statusProvider ?? throw new ArgumentNullException(nameof(statusProvider));
            Slug = slug ?? throw new ArgumentNullException(nameof(slug));
            AuthorId = authorId;
        }

        private Notification()
        {
        //used by EF Core
        }

        public Webhook Webhook { get; private set; }

        public string StatusProvider { get; private set; }

        public string Slug { get; private set; }

        public Guid AuthorId { get; private set; }

        /// <summary>
        /// Gets the last build number for which a notification has been sent
        /// </summary>
        public uint LastBuildNumber { get; private set; }
    }
}
