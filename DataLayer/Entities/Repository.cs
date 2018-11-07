using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Repository
    {
        public Guid Id { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public RepoProvider RepoProvider { get; set; }
    }
}
