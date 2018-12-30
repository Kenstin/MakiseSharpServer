using System;
using System.Threading.Tasks;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> AddAsync(User user);

        void Update(User user);

        Task<User> GetAsync(Guid userId);

        Task<User> GetByDiscordId(ulong discordId);
    }
}
