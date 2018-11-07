using System;

namespace DataLayer.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }

        public string Token { get; private set; }

        public RefreshToken(string token)
        {
            Token = token;
        }
    }
}
