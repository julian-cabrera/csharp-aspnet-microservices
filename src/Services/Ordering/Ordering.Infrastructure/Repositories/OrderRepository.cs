using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persitence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext dbcontetx)
            : base(dbcontetx) { }

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            return await _dbContext.Orders
                .Where(x => x.UserName == userName)
                .ToListAsync();
        }
    }
}
