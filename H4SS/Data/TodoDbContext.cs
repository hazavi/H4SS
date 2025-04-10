using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace H4SS.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cpr> Cpr { get; set; }
        public DbSet<Todolist> Todolist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the foreign key relationship
            modelBuilder.Entity<Todolist>()
                .HasOne(t => t.Cpr) // Each Todolist is associated with one Cpr
                .WithMany()        // A Cpr can have many Todolists (no navigation property in Cpr)
                .HasForeignKey(t => t.UserId) // Foreign key in Todolist
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete behavior
        }
    }
}