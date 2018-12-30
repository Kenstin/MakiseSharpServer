using System.Collections.Generic;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        private readonly List<RefreshToken> appRefreshTokens;

        public User(ulong discordId, string discordAccessToken, string discordRefreshToken)
        {
            DiscordId = discordId;
            DiscordAccessToken = discordAccessToken;
            DiscordRefreshToken = discordRefreshToken;

            appRefreshTokens = new List<RefreshToken>();
        }

        public ulong DiscordId { get; private set; }

        public string DiscordAccessToken { get; private set; }

        public string DiscordRefreshToken { get; private set; }
        //TODO: these types are not string - they are tokens, should they have their own class?

        public IReadOnlyCollection<RefreshToken> AppRefreshTokens => appRefreshTokens.AsReadOnly();

        public RefreshToken AddRefreshToken()
        {
            var refreshToken = RefreshToken.GenerateToken();
            appRefreshTokens.Add(refreshToken);
            return refreshToken;
        }

        public void ChangeDiscordCredentials(string accessToken, string refreshToken)
        {
            DiscordAccessToken = accessToken;
            DiscordRefreshToken = refreshToken;
        }
    }
}
