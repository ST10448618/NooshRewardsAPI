using NooshRewardsApi.Models;

namespace NooshRewardsApi.Repositories.Interfaces
{
    public interface IReceiptSubmissionRepository
    {
        Task<bool> ExistsAsync(string receiptReference, decimal amountPaid, DateOnly purchaseDate);
        Task AddAsync(ReceiptSubmission submission);
    }
}