namespace ServiceLayer.JwtServices
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
