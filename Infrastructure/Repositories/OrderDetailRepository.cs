using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderDetailRepository : BaseRepository<OrderDetailEntity>
    {
        public OrderDetailRepository(DataContext context) : base(context)
        {
        }

        //retrieve order details for a specific order
        public async Task<IEnumerable<OrderDetailEntity>> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Set<OrderDetailEntity>()
                                 .Where(od => od.OrderId == orderId)
                                 .ToListAsync();
        }
    }
}
