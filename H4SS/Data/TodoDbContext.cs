using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace H4SS.Data
{
    public partial class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cpr> Cprs { get; set; }
        public virtual DbSet<Todolist> Todolists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cpr>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Cpr__3214EC07B603CEB1");

                entity.ToTable("Cpr");

                entity.Property(e => e.CprNr).HasMaxLength(500);
                entity.Property(e => e.User).HasMaxLength(200);
            });

            modelBuilder.Entity<Todolist>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Todolist__3214EC07191F8783");

                entity.ToTable("Todolist");

                entity.Property(e => e.Item).HasMaxLength(500);

                entity.HasOne(d => d.User).WithMany(p => p.Todolists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Todolist_Cpr");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}