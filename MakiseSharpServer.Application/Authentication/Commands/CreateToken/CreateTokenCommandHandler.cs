using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MakiseSharpServer.Application.ApiClients.DiscordApi;
using MakiseSharpServer.Application.ApiClients.Errors;
using MakiseSharpServer.Application.Authentication.Models;
using MakiseSharpServer.Application.Authentication.Services;
using MakiseSharpServer.Application.Settings;
using MakiseSharpServer.Common;
using MediatR;

namespace MakiseSharpServer.Application.Authentication.Commands.CreateToken
{
    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, Result<JwtResponse>>
    {
        private readonly IDiscordApi discordApi;
        private readonly IDiscordJwtCreator jwtCreator;
        private readonly ITokenFactory tokenFactory;
        private readonly AppSettings appSettings;

        public CreateTokenCommandHandler(IDiscordApi discordApi, IDiscordJwtCreator jwtCreator, ITokenFactory tokenFactory, AppSettings appSettings)
        {
            this.discordApi = discordApi ?? throw new ArgumentNullException(nameof(discordApi));
            this.jwtCreator = jwtCreator ?? throw new ArgumentNullException(nameof(jwtCreator));
            this.tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            //TODO: is injecting AppSettings here is necessary and correct?
        }

        public async Task<Result<JwtResponse>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            DiscordToken token;
            try
            {
                token = await discordApi.GetAccessTokenAsync(
                    new DiscordAccessTokenRequestDto(
                        request.DiscordClientId, request.DiscordClientSecret, request.DiscordAccessToken, request.DiscordRedirectUri));
            }
            catch (Refit.ApiException e)
            {
                switch (e.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return new WrongAccessCodeError().AsResult<JwtResponse>();
                    case HttpStatusCode code when code.IsUnavailable():
                        return new UnavailableError().AsResult<JwtResponse>();
                    default:
                        throw;
                }
            }

            //TODO: does it even make sense to split those requests in two catches?
            DiscordUser user;
            try
            {
                user = await discordApi.GetBasicUserInfoBearerAsync(token.AccessToken);
            }
            catch (Refit.ApiException e)
            {
                switch (e.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        return new ForbiddenError().AsResult<JwtResponse>();
                    case HttpStatusCode code when code.IsUnavailable():
                        return new UnavailableError().AsResult<JwtResponse>();
                    default:
                        throw;
                }
            }

            //ToDo: save access&refresh token to db

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtCreator.FromUser(user));
            var response = new JwtResponse(new AccessToken(jwt, appSettings.Token.TokenLifetime), tokenFactory.GenerateToken());
            return new Result<JwtResponse>(response);
        }
    }
}