using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persitence
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);
    }
}
