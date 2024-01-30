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
    public class CustomerRepository : BaseRepository<CustomerEntity>
    {
        public CustomerRepository(DataContext context) : base(context)
        {
        }

        //Retrive customers by email 
        public async Task<CustomerEntity> GetCustomerByEmailAsync(string email )
        {
           if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            return await _context.Set<CustomerEntity>()
                 .Include(c => c.Orders)
                 .FirstOrDefaultAsync(c => c.Email == email);
        }

        //Retrive customers by name
        public async Task<IEnumerable<CustomerEntity>> GetCustomersByNameAsync(string firstName, string lastName)
        {

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return Enumerable.Empty<CustomerEntity>();
            }

            return await _context.Set<CustomerEntity>()
                                 .Where(c => c.FirstName == firstName && c.LastName == lastName)
                                 .ToListAsync();
        }
        // Retrieve all customers
        public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
        {
            return await _context.Set<CustomerEntity>().ToListAsync();
        }

        // Update an existing customer
        public async Task<bool> UpdateCustomerAsync(CustomerEntity customer)
        {
            var existingCustomer = await _context.Set<CustomerEntity>().FindAsync(customer.CustomerId);
            if (existingCustomer != null)
            {
                // Map updated values onto existing customer entity or attach and mark as modified
                _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        // Delete a customer by ID
        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Set<CustomerEntity>().FindAsync(customerId);
            if (customer != null)
            {
                _context.Set<CustomerEntity>().Remove(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //Get all orders for specific customer
        public async Task<IEnumerable<OrderEntity>> GetCustomerOrdersAsync(int customerId)
        {
            var customer = await _context.Set<CustomerEntity>()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return customer?.Orders ?? Enumerable.Empty<OrderEntity>();
        }

    }
}
