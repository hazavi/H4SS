using H4SS.Data;
using H4SS.Codes;
using Microsoft.EntityFrameworkCore;
using H4SS.Models;

namespace H4SS.Services
{
    public class TodoService
    {
        private readonly TodoDbContext _context;
        private readonly AsymmetriskEncryption _encryption;

        public TodoService(TodoDbContext context, AsymmetriskEncryption encryption)
        {
            _context = context;
            _encryption = encryption;
        }

        // Add this new method to TodoService.cs
        public async Task ToggleTodoStatusAsync(int todoId, string userName, bool isDone)
        {
            var todo = await _context.Todolists
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == todoId && t.User.User == userName);

            if (todo == null)
                throw new Exception("Todo item not found or does not belong to the user.");

            // Get the existing decrypted text
            var decryptedItem = await _encryption.DecryptAsymetrisk(todo.Item);

            // Create a new TodoItem to parse the content
            var todoItem = new TodoItem
            {
                Id = todo.Id,
                Text = decryptedItem
            };

            // Format the string to include completion status
            // We'll use a prefix to indicate the status: "DONE:" or "" for not done
            string prefixedText = isDone ? $"DONE:{todoItem.Text}" : todoItem.Text.Replace("DONE:", "");

            // Re-encrypt and save
            todo.Item = await _encryption.EncryptAsymetrisk(prefixedText);
            await _context.SaveChangesAsync();
        }

        // Also modify the GetTodosAsync method to handle the status
        public async Task<List<TodoItem>> GetTodosAsync(string userName)
        {
            var userCpr = await _context.Cprs.FirstOrDefaultAsync(c => c.User == userName);
            if (userCpr == null)
            {
                throw new Exception("CPR entry not found for the user.");
            }

            var encryptedTodos = await _context.Todolists
                .Where(t => t.UserId == userCpr.Id)
                .ToListAsync();

            var todoItems = new List<TodoItem>();
            foreach (var todo in encryptedTodos)
            {
                var decryptedItem = await _encryption.DecryptAsymetrisk(todo.Item);
                bool isDone = decryptedItem.StartsWith("DONE:");
                string text = isDone ? decryptedItem.Substring(5) : decryptedItem;

                todoItems.Add(new TodoItem
                {
                    Id = todo.Id,
                    Text = text,
                    IsDone = isDone
                });
            }

            return todoItems;
        }


        // Add a new Todo item for a user
        public async Task AddTodoAsync(string userName, string todoItem)
        {
            if (string.IsNullOrWhiteSpace(todoItem))
                throw new ArgumentException("Todo item cannot be empty.", nameof(todoItem));

            try
            {
                var userCpr = await _context.Cprs.FirstOrDefaultAsync(c => c.User == userName);
                if (userCpr == null)
                    throw new Exception("CPR entry not found for the user.");

                // Ensure the UserId is valid
                if (!_context.Cprs.Any(c => c.Id == userCpr.Id))
                    throw new Exception("Invalid UserId for the Todo item.");

                var encryptedItem = await _encryption.EncryptAsymetrisk(todoItem);

                var newTodo = new Todolist
                {
                    UserId = userCpr.Id,
                    Item = encryptedItem
                };

                _context.Todolists.Add(newTodo);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in AddTodoAsync: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        // Edit a Todo item by ID
        public async Task EditTodoAsync(int todoId, string userName, string newTodoItem)
        {
            if (string.IsNullOrWhiteSpace(newTodoItem))
                throw new ArgumentException("Todo item cannot be empty.", nameof(newTodoItem));

            var todo = await _context.Todolists
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == todoId && t.User.User == userName);

            if (todo == null)
                throw new Exception("Todo item not found or does not belong to the user.");

            todo.Item = await _encryption.EncryptAsymetrisk(newTodoItem);
            await _context.SaveChangesAsync();
        }

        // Delete a Todo item by ID
        public async Task DeleteTodoAsync(int todoId, string userName)
        {
            var todo = await _context.Todolists
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == todoId && t.User.User == userName);

            if (todo == null)
                throw new Exception("Todo item not found or does not belong to the user.");

            _context.Todolists.Remove(todo);
            await _context.SaveChangesAsync();
        }
    }
}
