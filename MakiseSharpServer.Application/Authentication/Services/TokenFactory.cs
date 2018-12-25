using System;
using System.Security.Cryptography;

namespace MakiseSharpServer.Application.Authentication.Services
{
    public class TokenFactory : ITokenFactory
    {
        public string GenerateToken(uint size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}