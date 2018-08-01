using System.IdentityModel.Tokens.Jwt;
using MakiseSharpServer.Models.Discord;

namespace MakiseSharpServer.Services
{
    public interface IDiscordJwtCreator
    {
        JwtSecurityToken FromUser(DiscordUser user);
    }
}