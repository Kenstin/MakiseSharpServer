namespace MakiseSharpServer.API.Models.Settings
{
    public class AppSettings
    {
        public DiscordSettings Discord { get; set; }

        public TokenSettings Token { get; set; }
    }
}
