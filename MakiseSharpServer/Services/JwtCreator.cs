using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MakiseSharpServer.Models.Discord;
using MakiseSharpServer.Models.Settings;
using Microsoft.IdentityModel.Tokens;

namespace MakiseSharpServer.Services
{
    public class JwtCreator : IDiscordJwtCreator
    {
        private readonly AppSettings appSettings;
        private readonly SymmetricSecurityKey key;

        public JwtCreator(AppSettings appSettings)
        {
            this.appSettings = appSettings;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Token.SigningKey));
        }

        public JwtSecurityToken Create(IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: appSettings.Token.Issuer,
                audience: appSettings.Token.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(appSettings.Token.TokenLifetime),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        }

        public JwtSecurityToken FromUser(DiscordUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(appSettings.Token.Claims.DiscordUserId, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(appSettings.Token.Claims.DiscordUserDiscriminator, user.Discriminator),
                new Claim(appSettings.Token.Claims.DiscordUserAvatar, user.Avatar),
            };

            return Create(claims);
        }
    }
}
