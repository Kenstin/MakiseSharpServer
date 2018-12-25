namespace MakiseSharpServer.Application.Authentication.Models
{
    public class AccessToken
    {
        public AccessToken(string token, uint expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }

        public string Token { get; }

        public uint ExpiresIn { get; }
    }
}