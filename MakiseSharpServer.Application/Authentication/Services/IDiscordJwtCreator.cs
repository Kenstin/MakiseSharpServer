using System.IdentityModel.Tokens.Jwt;
using MakiseSharpServer.Application.Authentication.Models;

namespace MakiseSharpServer.Application.Authentication.Services
{
    public interface IDiscordJwtCreator
    {
        JwtSecurityToken FromUser(DiscordUser user);
    }
}