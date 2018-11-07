namespace ServiceLayer.Models.Jwt
{
    public class JwtResponse
    {
        public AccessToken AccessToken { get; }

        public string RefreshToken { get; }

        public JwtResponse(AccessToken accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
