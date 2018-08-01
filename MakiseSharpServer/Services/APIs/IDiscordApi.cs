using System.Threading.Tasks;
using MakiseSharpServer.Models.Discord;
using Microsoft.AspNetCore.Http;
using Refit;

namespace MakiseSharpServer.Services.APIs
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