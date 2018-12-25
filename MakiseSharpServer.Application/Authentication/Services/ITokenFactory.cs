namespace MakiseSharpServer.Application.Authentication.Services
{
    public interface ITokenFactory
    {
        string GenerateToken(uint size = 32);
    }
}