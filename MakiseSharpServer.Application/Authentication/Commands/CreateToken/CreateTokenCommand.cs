using System;
using MakiseSharpServer.Application.Authentication.Models;
using MakiseSharpServer.Common;
using MediatR;

namespace MakiseSharpServer.Application.Authentication.Commands.CreateToken
{
    public class CreateTokenCommand : IRequest<Result<JwtResponse>>
    {
        public CreateTokenCommand(string discordAccessToken, ulong discordClientId, string discordClientSecret, Uri discordRedirectUri)
        {
            DiscordAccessToken = discordAccessToken;
            DiscordClientId = discordClientId;
            DiscordClientSecret = discordClientSecret;
            DiscordRedirectUri = discordRedirectUri;
        }

        public string DiscordAccessToken { get; }

        public ulong DiscordClientId { get; }

        public string DiscordClientSecret { get; }

        public Uri DiscordRedirectUri { get; }
    }
}