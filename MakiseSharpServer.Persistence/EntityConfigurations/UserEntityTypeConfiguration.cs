using MakiseSharpServer.Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakiseSharpServer.Persistence.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.DiscordId).IsUnique();

            builder.Property(u => u.DiscordId).IsRequired();
            builder.Property(u => u.DiscordAccessToken).IsRequired();
            builder.Property(u => u.DiscordRefreshToken).IsRequired();
        }
    }
}