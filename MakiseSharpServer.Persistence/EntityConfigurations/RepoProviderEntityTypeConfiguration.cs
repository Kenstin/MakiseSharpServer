using System;
using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakiseSharpServer.Persistence.EntityConfigurations
{
    public class RepoProviderEntityTypeConfiguration : IEntityTypeConfiguration<RepoProvider>
    {
        public void Configure(EntityTypeBuilder<RepoProvider> builder)
        {
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.ApiUri).HasConversion(u => u.ToString(), u => new Uri(u)).IsRequired();
        }
    }
}
