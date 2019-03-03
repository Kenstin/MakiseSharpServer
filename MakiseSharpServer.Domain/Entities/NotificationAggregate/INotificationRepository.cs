using System.Threading.Tasks;
using MakiseSharpServer.Domain.SeedWork;

namespace MakiseSharpServer.Domain.Entities.NotificationAggregate
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<Notification> AddAsync(Notification notification);

        void Update(Notification notification);

        Task RemoveAsync(Notification notification);
    }
}
