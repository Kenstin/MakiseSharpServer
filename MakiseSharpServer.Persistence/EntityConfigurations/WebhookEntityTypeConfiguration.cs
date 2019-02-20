using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakiseSharpServer.Persistence.EntityConfigurations
{
    public class WebhookEntityTypeConfiguration : IEntityTypeConfiguration<Webhook>
    {
        public void Configure(EntityTypeBuilder<Webhook> builder)
        {
            builder.HasIndex(w => w.WebhookDiscordId).IsUnique();

            builder.Property(w => w.WebhookDiscordToken).IsRequired();
        }
    }
}
