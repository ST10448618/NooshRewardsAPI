using Microsoft.EntityFrameworkCore;
using NooshRewardsApi.Data;
using NooshRewardsApi.Models;
using NooshRewardsApi.Repositories.Interfaces;

namespace NooshRewardsApi.Repositories
{
    public class ReceiptSubmissionRepository : IReceiptSubmissionRepository
    {
        private readonly RewardsDbContext _context;
        public ReceiptSubmissionRepository(RewardsDbContext context) { _context = context; }

        public async Task<bool> ExistsAsync(string receiptReference, decimal amountPaid, DateOnly purchaseDate) =>
            await _context.ReceiptSubmissions.AnyAsync(r =>
                r.ReceiptReference == receiptReference &&
                r.AmountPaid == amountPaid &&
                r.PurchaseDate == purchaseDate);

        public async Task AddAsync(ReceiptSubmission submission)
        {
            await _context.ReceiptSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();
        }
    }
}