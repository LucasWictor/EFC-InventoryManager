using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllCustomersAsync();
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetOneAsync(c => c.CustomerId == id);
        }

        public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)
        {
            return await _customerRepository.CreateAsync(customer);
        }

        public async Task<bool> UpdateCustomerAsync(CustomerEntity customer)
        {
            return await _customerRepository.UpdateCustomerAsync(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            return await _customerRepository.DeleteCustomerAsync(customerId);
        }
    }
}