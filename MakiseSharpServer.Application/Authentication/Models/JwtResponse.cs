namespace MakiseSharpServer.Application.Authentication.Models
{
    public class JwtResponse
    {
        public JwtResponse(AccessToken accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public AccessToken AccessToken { get; }

        public string RefreshToken { get; }
    }
}