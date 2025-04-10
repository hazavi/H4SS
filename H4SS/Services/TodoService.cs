//using H4SS.Data;
//using Microsoft.EntityFrameworkCore;

//namespace H4SS.Services
//{
//    public class TodoService
//    {
//        private readonly TodoDbContext _context;

//        public TodoService(TodoDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Cpr?> GetCprByUserAsync(string userId)
//        {
//            return await _context.Cpr.FirstOrDefaultAsync(c => c.User == userId);
//        }

//        public async Task SaveCprAsync(string userId, string cprNr)
//        {
//            var cpr = new Cpr { User = userId, CprNr = cprNr };
//            _context.Cpr.Add(cpr);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<bool> ValidateCprAsync(string userId, string cprNr)
//        {
//            var cpr = await _context.Cpr.FirstOrDefaultAsync(c => c.User == userId);
//            return cpr != null && cpr.CprNr == cprNr;
//        }

//        public async Task<List<Todolist>> GetTodosByUserAsync(int userId)
//        {
//            return await _context.Todolist.Where(t => t.UserId == userId).ToListAsync();
//        }

//        public async Task AddTodoAsync(int userId, string item)
//        {
//            var todo = new Todolist { UserId = userId, Item = item };
//            _context.Todolist.Add(todo);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
