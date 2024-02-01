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
        //list all for reporting or administrative purposes
        public async Task<IEnumerable<OrderDetailEntity>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails.ToListAsync();
        }
        // Add items to an existing order
        public async Task<OrderDetailEntity> AddOrderDetailAsync(OrderDetailEntity orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }
        //change the quantity of an item in an order or update other details
        public async Task<bool> UpdateOrderDetailAsync(OrderDetailEntity orderDetail)
        {
            var existingOrderDetail = await _context.OrderDetails.FindAsync(orderDetail.OrderDetailId);
            if (existingOrderDetail != null)
            {
                _context.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //Delete to remove an itemn fron an order
        public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(orderDetailId);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
