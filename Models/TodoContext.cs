using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<TodoItem>()
                .HasOne(u => u.User)
                .WithMany(t => t.TodoItems)
                .HasForeignKey(t => t.User_Id);
        }

    }
}