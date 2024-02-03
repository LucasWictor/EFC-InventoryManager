
using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly OrderDetailRepository _orderDetailRepository; // Ensure this line is added

        public OrderService(OrderRepository orderRepository, OrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository; // Ensure this assignment is added
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
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error)
                return false;
            }
            return false;
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
