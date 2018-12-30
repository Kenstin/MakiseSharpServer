using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using MakiseSharpServer.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace MakiseSharpServer.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "db is short for database")]
        private readonly MakiseDbContext dbContext;

        public UserRepository(MakiseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => dbContext;

        public async Task<User> AddAsync(User user)
        {
            var result = await dbContext.AddAsync(user);
            return result.Entity;
        }

        public void Update(User user)
        {
            dbContext.Update(user); //state = Modified
        }

        public async Task<User> GetAsync(Guid userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetByDiscordId(ulong discordId)
        {
            return await dbContext.Users.Where(u => u.DiscordId == discordId).SingleOrDefaultAsync();
        }
    }
}
