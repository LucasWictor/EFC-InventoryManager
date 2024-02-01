using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepository;

        public OrderDetailService(OrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<OrderDetailEntity> AddOrderDetailAsync(OrderDetailEntity orderDetail)
        {
            // Add business logic and validations 
            return await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
        }

        public async Task<bool> UpdateOrderDetailAsync(OrderDetailEntity orderDetail)
        {
            // Add business logic and validations 
            return await _orderDetailRepository.UpdateOrderDetailAsync(orderDetail);
        }

        public async Task<bool> DeleteOrderDetailAsync(int orderDetailId)
        {
            // Add business logic and validations 
            return await _orderDetailRepository.DeleteOrderDetailAsync(orderDetailId);
        }

        public async Task<IEnumerable<OrderDetailEntity>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            // Add business logic and validations 
            return await _orderDetailRepository.GetOrderDetailsAsync(orderId);
        }
    }
}