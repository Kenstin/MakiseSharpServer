using System;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public class RepoProvider : Entity
    {
        private RepoProvider()
        {
        }

        public string Name { get; private set; }

        public Uri ApiUri { get; private set; }
    }
}
