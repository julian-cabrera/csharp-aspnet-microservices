using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderDbContextSeed
    {
        public static async Task SeedAsync(OrderDbContext orderContext, ILogger<OrderDbContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderDbContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order()
                {
                    UserName = "admin", 
                    FirstName = "John", 
                    LastName = "Doe", 
                    EmailAddress = "admin@aspnet.com", 
                    AddressLine = "Mockstreet Av", 
                    Country = "USA",
                    TotalPrice = 350
                }
            };
        }
    }
}