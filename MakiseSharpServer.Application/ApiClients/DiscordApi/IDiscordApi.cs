using System.Threading.Tasks;
using MakiseSharpServer.Application.Authentication;
using MakiseSharpServer.Application.Authentication.Models;
using Refit;

namespace MakiseSharpServer.Application.ApiClients.DiscordApi
{
    public interface IDiscordApi
    {
        [Post("/oauth2/token")]
        Task<DiscordToken> GetAccessTokenAsync([Body(BodySerializationMethod.UrlEncoded)]
            DiscordAccessTokenRequestDto discordAccessTokenRequestDto);

        [Get("/users/@me")]
        Task<DiscordUser> GetBasicUserInfoAsync([Header("Authorization")] string accessToken);
    }
}
