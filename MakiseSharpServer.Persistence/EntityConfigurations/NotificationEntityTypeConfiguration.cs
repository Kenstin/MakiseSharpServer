using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakiseSharpServer.Persistence.EntityConfigurations
{
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.OwnsOne(n => n.Webhook);
            builder.Property(n => n.StatusProvider).IsRequired();
            builder.Property(n => n.Slug).IsRequired();
            builder.Property(n => n.AuthorId).IsRequired();
        }
    }
}
