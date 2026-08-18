using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Data;
using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;

namespace NooshRewardsApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RewardsDbContext _context;
        public CustomerRepository(RewardsDbContext context) { _context = context; }

        public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber) =>
            await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);

        public async Task<Customer> CreateAsync(string phoneNumber, string? fullName)
        {
            var customer = new Customer { PhoneNumber = phoneNumber, FullName = fullName };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}