
using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<OrderEntity> CreateOrderAsync(OrderEntity order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int OrderId, string NewStatus)
        {
            return await _orderRepository.UpdateOrderStatusAsync(OrderId, NewStatus);
        }

        public async Task<IEnumerable<OrderEntity>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
    }
}
