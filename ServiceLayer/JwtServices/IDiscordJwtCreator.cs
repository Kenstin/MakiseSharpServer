using System.IdentityModel.Tokens.Jwt;
using ServiceLayer.Models.Discord;

namespace ServiceLayer.JwtServices
{
    public interface IDiscordJwtCreator
    {
        JwtSecurityToken FromUser(DiscordUser user);
    }
}