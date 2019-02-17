using System;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        public User(ulong discordId, string discordAccessToken, string discordRefreshToken)
        {
            DiscordId = discordId;
            DiscordAccessToken = discordAccessToken ?? throw new ArgumentNullException(nameof(discordAccessToken));
            DiscordRefreshToken = discordRefreshToken ?? throw new ArgumentNullException(nameof(discordRefreshToken));
        }

        public ulong DiscordId { get; private set; }

        public string DiscordAccessToken { get; private set; }

        public string DiscordRefreshToken { get; private set; }
        //TODO: these types are not string - they are tokens, should they have their own class?

        public void ChangeDiscordCredentials(string accessToken, string refreshToken)
        {
            DiscordAccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            DiscordRefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
        }
    }
}
