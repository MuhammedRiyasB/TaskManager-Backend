using Microsoft.EntityFrameworkCore;
using TaskManager.Modules.Tasks.Models;

namespace TaskManager.Modules.Tasks.Data;

public class TasksDbContext : DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options)
        : base(options)
    {
    }

    // set per request from API
    public int? CurrentUserId { get; set; }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.UserId)
                  .IsRequired();

            entity.HasIndex(x => x.UserId);
                  

            // ✅ automatic user isolation
            entity.HasQueryFilter(t =>
                !CurrentUserId.HasValue || t.UserId == CurrentUserId.Value);
        });
    }
}
