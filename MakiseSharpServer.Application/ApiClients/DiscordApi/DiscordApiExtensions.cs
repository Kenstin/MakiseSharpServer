using System.Threading.Tasks;
using MakiseSharpServer.Application.Authentication.Models;

namespace MakiseSharpServer.Application.ApiClients.DiscordApi
{
    public static class DiscordApiExtensions
    {
        public static Task<DiscordUser> GetBasicUserInfoBearerAsync(this IDiscordApi discordApi, string accessToken) => discordApi.GetBasicUserInfoAsync($"Bearer {accessToken}");
    }
}
