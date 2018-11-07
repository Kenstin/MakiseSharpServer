using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Entities
{
    [Owned]
    public class DiscordValues
    {
        [Required]
        public ulong Id { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
