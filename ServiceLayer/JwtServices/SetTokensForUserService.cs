using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.JwtServices
{
    public class SetTokensForUserService : ISetTokensForUserService
    {
        private readonly EfCoreContext context;

        public SetTokensForUserService(EfCoreContext context)
        {
            this.context = context;
        }

        public async Task SetTokensAsync(ulong discordId, string refreshToken, string accessToken, string appRefreshToken)
        {
            var user = await context.Users.Where(u => u.DiscordValues.Id == discordId).FirstAsync();
            if (user == null) //Make sure User exists
            {
                user = new User
                {
                    DiscordValues = new DiscordValues
                    {
                        Id = discordId
                    }
                };

                await context.AddAsync(user);
            }

            user.DiscordValues.RefreshToken = refreshToken;
            user.DiscordValues.AccessToken = accessToken;
            user.AppRefreshTokens.Add(new RefreshToken(refreshToken));

            await context.SaveChangesAsync();
        }
    }
}