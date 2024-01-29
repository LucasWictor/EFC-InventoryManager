using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<CustomerEntity>
    {
        public CustomerRepository(DbContext context ) : base(context)
        { 

        }

        //retrive customers by email 
        public async Task<CustomerEntity> GetCustomersByEmailAsync(string email )
        {
            return await GetOneAsync(c => c.Email == email);
        }

        //Retrive customers by name
        public async Task<IEnumerable<CustomerEntity>> GetCustomersByNameAsync(string firstName, string lastName)
        {
            return await GetAllAsync();
        }
        //Get all orders for specific customer
        public async Task<IEnumerable<OrderEntity>> GetCustomerOrdersAsync(int customerId)
        {
            var customer = await GetOneAsync(c => c.CustomerId == customerId);
            return customer?.Orders;
        }

    }
}
