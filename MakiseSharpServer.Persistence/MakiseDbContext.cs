using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Domain.Entities.NotificationAggregate;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using MakiseSharpServer.Domain.SeedWork;
using MakiseSharpServer.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MakiseSharpServer.Persistence
{
    public class MakiseDbContext : DbContext, IUnitOfWork
    {
        public MakiseDbContext(DbContextOptions<MakiseDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
        }
    }
}
