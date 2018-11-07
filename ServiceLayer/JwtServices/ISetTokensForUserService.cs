using System.Threading.Tasks;

namespace ServiceLayer.JwtServices
{
    public interface ISetTokensForUserService
    {
        Task SetTokensAsync(ulong discordId, string refreshToken, string accessToken, string appRefreshToken);
    }
}
