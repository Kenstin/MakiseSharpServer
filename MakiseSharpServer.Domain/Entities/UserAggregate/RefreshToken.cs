using System;
using System.Security.Cryptography;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.UserAggregate
{
    public class RefreshToken : Entity
    {
        private RefreshToken()
        {
        }

        public string Token { get; private set; }

        public static RefreshToken GenerateToken()
        {
            const int size = 32;
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var token = Convert.ToBase64String(randomNumber);
                var refreshToken = new RefreshToken { Token = token };
                return refreshToken;
            }
        }
    }
}