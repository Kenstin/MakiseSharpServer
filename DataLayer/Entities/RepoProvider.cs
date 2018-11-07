using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class RepoProvider
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ApiUri { get; set; } //System.Uri?
    }
}
