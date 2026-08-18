using NooshRewardsApi.Models;

namespace NooshRewardsApi.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Customer> CreateAsync(string phoneNumber, string? fullName);
    }
}