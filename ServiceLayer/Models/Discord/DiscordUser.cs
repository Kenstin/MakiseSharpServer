namespace ServiceLayer.Models.Discord
{
    public class DiscordUser
    {
        public ulong Id { get; set; }

        public string Username { get; set; }

        public string Discriminator { get; set; }

        /// <summary>
        /// Gets or sets the user's avatar hash
        /// </summary>
        public string Avatar { get; set; }
    }
}
