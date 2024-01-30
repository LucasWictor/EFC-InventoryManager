using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<OrderEntity>
    {
        public OrderRepository(DataContext context) : base(context)
        {
        }

        // GETALL orders for a specific customer
        public async Task<IEnumerable<OrderEntity>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Set<OrderEntity>()
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }

        // Update the status of an order
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Set<OrderEntity>().FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Get order details by orderID
        public async Task<OrderEntity> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Set<OrderEntity>()
                                 .Include(o => o.OrderDetails)
                                 .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

    }
}