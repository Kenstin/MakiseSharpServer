using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        [Required]
        public ulong WebhookId { get; set; }

        [Required]
        public string WebhookToken { get; set; }

        [Required]
        public Guid RepositoryId { get; set; }

        public Repository Repository { get; set; }

        /// <summary>
        /// Gets or sets the last build number for which a notification was sent
        /// </summary>
        public uint LastBuildNumber { get; set; }
    }
}
