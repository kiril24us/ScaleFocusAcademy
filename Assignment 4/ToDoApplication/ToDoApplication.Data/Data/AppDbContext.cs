using Microsoft.EntityFrameworkCore;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Data.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; }

        public DbSet<ToDoList> ToDoLists { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<UserToDoList> UsersToDoLists { get; set; }

        public DbSet<UserTask> UsersTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }

            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.TaskId });

                entity.HasOne(ut => ut.User)
                      .WithMany(u => u.UserSharedTasks)
                      .HasForeignKey(ut => ut.UserId)
                      .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(ut => ut.Task)
                      .WithMany(t => t.SharedTasks)
                      .HasForeignKey(ut => ut.TaskId)
                      .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<UserToDoList>(entity =>
            {
                entity.HasKey(ut => new { ut.UserId, ut.ToDoListId });

                entity.HasOne(ut => ut.User)
                      .WithMany(u => u.UserSharedLists)
                      .HasForeignKey(ut => ut.UserId)
                      .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(ut => ut.ToDoList)
                      .WithMany(td => td.ToDoListsUsers)
                      .HasForeignKey(ut => ut.ToDoListId)
                      .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<ToDoList>(entity =>
            {
                entity.HasOne(td => td.User)
                      .WithMany(u => u.ToDoLists)
                      .HasForeignKey(td => td.CreatedById)
                      .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(td => td.CreatedById)
                      .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(t => t.ToDoList)
                      .WithMany(u => u.Tasks)
                      .HasForeignKey(t => t.ToDoListId)
                      .OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}

