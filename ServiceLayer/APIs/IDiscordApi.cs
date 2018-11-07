using System.Threading.Tasks;
using Refit;
using ServiceLayer.Models.Discord;

namespace ServiceLayer.APIs
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