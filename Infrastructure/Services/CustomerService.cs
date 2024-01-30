using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerEntity> CreateCustomerAsync(CustomerEntity customer)

        {
            //might add additional business logic here before creating the customer
            return await _customerRepository.CreateAsync(customer);
        }

        public async Task<CustomerEntity> GetCustomerByIdAsync(int customerId)
        {
            return await _customerRepository.GetOneAsync(c => c.CustomerId == customerId);
        }

        public async Task<bool> UpdateCustomerAsync(CustomerEntity customer)
        {
            return await _customerRepository.UpdateAsync(customer);
        }
    }
}
