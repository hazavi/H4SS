using H4SS.Data;
using Microsoft.EntityFrameworkCore;

namespace H4SS.Services
{
    public class CprValidationService
    {
        private readonly TodoDbContext _context;
        private readonly Codes.HashingHandler _hashingHandler;

        public CprValidationService(TodoDbContext context)
        {
            _context = context;
            _hashingHandler = new Codes.HashingHandler();
        }

        // Check if the user's CPR is validated
        public async Task<bool> IsCprValidatedAsync(string userName)
        {
            // Query the database to check if the user has a CPR entry
            var userCpr = await _context.Cprs.FirstOrDefaultAsync(c => c.User == userName);
            return userCpr != null;
        }

        // Check if the user has a CPR number saved
        public async Task<bool> HasCprNumberAsync(string userName)
        {
            var userCpr = await _context.Cprs.FirstOrDefaultAsync(c => c.User == userName);
            return userCpr != null;
        }

        // Get the stored hashed CPR number for the user
        public async Task<string?> GetStoredCprAsync(string userName)
        {
            var userCpr = await _context.Cprs.FirstOrDefaultAsync(c => c.User == userName);
            return userCpr?.CprNr;
        }

        // Validate the entered CPR number against the stored hashed CPR number
        public async Task<bool> ValidateCprAsync(string userName, string enteredCpr)
        {
            var storedCpr = await GetStoredCprAsync(userName);
            if (string.IsNullOrEmpty(storedCpr))
            {
                return false;
            }

            return _hashingHandler.BCryptVerifyHashing2(enteredCpr, storedCpr);
        }

        // Save a new CPR number for the user
        public async Task SaveCprAsync(string userName, string cprNumber)
        {
            string hashedCpr = _hashingHandler.BCryptHashing2(cprNumber);

            var cprEntry = new Cpr
            {
                User = userName,
                CprNr = hashedCpr
            };

            _context.Cprs.Add(cprEntry);
            await _context.SaveChangesAsync();
        }
    }
}
