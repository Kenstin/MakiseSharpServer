using MakiseSharpServer.Application.Authentication.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MakiseSharpServer.API.Swagger.Examples
{
    public class JwtResponseExample : IExamplesProvider<JwtResponse>
    {
        public JwtResponse GetExamples()
        {
            var accessToken = new AccessToken("abcdxyz123", 5);
            return new JwtResponse(accessToken, "qwertyu123");
        }
    }
}
