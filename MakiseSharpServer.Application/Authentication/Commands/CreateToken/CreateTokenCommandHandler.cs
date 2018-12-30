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
using MakiseSharpServer.Domain.Entities.UserAggregate;
using MediatR;

namespace MakiseSharpServer.Application.Authentication.Commands.CreateToken
{
    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, Result<JwtResponse>>
    {
        private readonly IDiscordApi discordApi;
        private readonly IDiscordJwtCreator jwtCreator;
        private readonly IUserRepository userRepository;
        private readonly AppSettings appSettings;

        public CreateTokenCommandHandler(IDiscordApi discordApi, IDiscordJwtCreator jwtCreator, IUserRepository userRepository, AppSettings appSettings)
        {
            this.discordApi = discordApi ?? throw new ArgumentNullException(nameof(discordApi));
            this.jwtCreator = jwtCreator ?? throw new ArgumentNullException(nameof(jwtCreator));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            //TODO: is injecting AppSettings here is necessary and correct?
        }

        public async Task<Result<JwtResponse>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
        {
            DiscordToken discordToken;
            try
            {
                discordToken = await discordApi.GetAccessTokenAsync(
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
            DiscordUser discordUser;
            try
            {
                discordUser = await discordApi.GetBasicUserInfoBearerAsync(discordToken.AccessToken);
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

            var apiUser = await userRepository.GetByDiscordId(discordUser.Id);

            if (apiUser is null) //if User is new
            {
                apiUser = new User(discordUser.Id, discordToken.AccessToken, discordToken.RefreshToken);
                apiUser = await userRepository.AddAsync(apiUser);
            }
            else //if User is in db
            {
                apiUser.ChangeDiscordCredentials(discordToken.AccessToken, discordToken.RefreshToken);
            }

            var apiRefreshToken = apiUser.AddRefreshToken();
            await userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtCreator.FromUser(discordUser));
            var response = new JwtResponse(new AccessToken(jwt, appSettings.Token.TokenLifetime), apiRefreshToken.Token);
            return new Result<JwtResponse>(response);
        }
    }
}