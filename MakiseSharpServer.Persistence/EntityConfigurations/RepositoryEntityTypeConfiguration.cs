using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakiseSharpServer.Persistence.EntityConfigurations
{
    public class RepositoryEntityTypeConfiguration : IEntityTypeConfiguration<Repository>
    {
        public void Configure(EntityTypeBuilder<Repository> builder)
        {
            builder.Property(r => r.Slug).IsRequired();
            builder.Property(r => r.RepoProvider).IsRequired();
        }
    }
}
