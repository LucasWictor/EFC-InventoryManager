using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderDetailRepository _orderDetailRepository;
        private readonly DataContext _context;

        public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository, DataContext context)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _context = context;
        }

        public async Task<bool> CreateOrderAsync(OrderEntity order, List<OrderDetailEntity> orderDetails)
        {
            try
            {
                var createdOrder = await _orderRepository.CreateAsync(order);
                if (createdOrder != null)
                {
                    foreach (var detail in orderDetails)
                    {
                        detail.OrderId = createdOrder.OrderId;
                        await _orderDetailRepository.AddOrderDetailAsync(detail);
                    }
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
            return false;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                return await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
            }
            catch (Exception ex)
            {
                 
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<OrderEntity>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepository.GetAllOrdersAsync();
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Enumerable.Empty<OrderEntity>();
            }
        }

        public async Task<bool> CustomerHasOrdersAsync(int customerId)
        {
            try
            {
                var orders = await _context.Set<OrderEntity>().Where(o => o.CustomerId == customerId).ToListAsync();
                return orders.Any();
            }
            catch (Exception ex)
            {
                //log the exception 
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrdersByCustomerIdAsync(int customerId)
        {
            try
            {
                var orders = await _context.Set<OrderEntity>().Where(o => o.CustomerId == customerId).ToListAsync();
                _context.Set<OrderEntity>().RemoveRange(orders);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
            
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Set<OrderEntity>().FindAsync(orderId);
                if (order != null)
                {
                    _context.Set<OrderEntity>().Remove(order);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
            
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}