using System;
using System.Collections.Generic;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public class Repository : ValueObject
    {
        public Repository(string slug, RepoProvider repoProvider)
        {
            Slug = slug ?? throw new ArgumentNullException(nameof(slug));
            RepoProvider = repoProvider ?? throw new ArgumentNullException(nameof(repoProvider));
        }

        public string Slug { get; private set; }

        public RepoProvider RepoProvider { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Slug;
        }
    }
}
