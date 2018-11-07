using System;
using Newtonsoft.Json;
using ServiceLayer.Converters;

namespace ServiceLayer.Models.Discord
{
    public class DiscordToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        [JsonConverter(typeof(ExpiryDateConverter))]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
