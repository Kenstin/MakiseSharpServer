namespace ServiceLayer.Models.Jwt
{
    public sealed class AccessToken
    {
        public string Token { get; }

        public uint ExpiresIn { get; }

        public AccessToken(string token, uint expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
