using Microsoft.EntityFrameworkCore;
using VP_ToDo_App.Domain.Entities;

namespace VP_ToDo_App.Infrastructure.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    public DbSet<TodoTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoTask>(entity =>
        {
            entity.ToTable("Tasks");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.Deadline)
                .IsRequired(false);
            entity.Property(e => e.UpdatedAt)
                .IsRequired(false);
            
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Deadline);
        });
    }
}
