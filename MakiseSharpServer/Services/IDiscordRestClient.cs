using System;
using System.Threading.Tasks;
using MakiseSharpServer.Models.Discord;

namespace MakiseSharpServer.Services
{
    public interface IDiscordRestClient
    {
        Task<DiscordTokenResponse> GetAccessTokenAsync(string accessCode, ulong clientId, string clientSecret, Uri redirectUri);
        Task<DiscordUser> GetBasicUserInfoAsync(string accessToken);
    }
}
