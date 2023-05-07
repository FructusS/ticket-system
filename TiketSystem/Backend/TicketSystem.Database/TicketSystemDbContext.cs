using Microsoft.EntityFrameworkCore;
using TicketSystem.Backend.Models;

namespace TicketSystem.Database;

public partial class TicketSystemDbContext : DbContext
{


    public TicketSystemDbContext(DbContextOptions<TicketSystemDbContext> options)
        : base(options)
    {
        
    }

    public virtual DbSet<Backend.Models.Task> Task { get; set; }

    public virtual DbSet<Backend.Models.TaskStatus> TaskStatus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Backend.Models.Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pk");

            entity.ToTable("task");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('task_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Cabinet).HasColumnName("cabinet");
            entity.Property(e => e.CompletedAt)
           .HasColumnType("timestamp without time zone")
           .HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                   .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TaskStatusId).HasColumnName("task_status_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Tasks)
               .HasForeignKey(d => d.TaskStatusId)
               .HasConstraintName("task_task_status_fk");

        });

        modelBuilder.Entity<Backend.Models.TaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_status_pk");

            entity.ToTable("task_status");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('task_status_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });
        modelBuilder.HasSequence("task_seq");
        modelBuilder.HasSequence("task_status_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
