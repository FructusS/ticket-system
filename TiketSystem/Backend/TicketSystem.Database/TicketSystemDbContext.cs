using Microsoft.EntityFrameworkCore;
using TicketSystem.Database.Models;
using Task = TicketSystem.Database.Models.Task;
using TaskStatus = TicketSystem.Database.Models.TaskStatus;


namespace TicketSystem.Database;

public partial class TicketSystemDbContext : DbContext
{


   public TicketSystemDbContext()
    {
    }

    public TicketSystemDbContext(DbContextOptions<TicketSystemDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pk");

            entity.ToTable("task");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('task_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Cabinet).HasColumnName("cabinet");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TaskStatusId).HasColumnName("task_status_id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.TaskStatus).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskStatusId)
                .HasConstraintName("task_task_status_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("task_user_fk");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_status_pk");

            entity.ToTable("task_status");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('task_status_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("nextval('user_seq'::regclass)")
                .HasColumnName("user_id");
            entity.Property(e => e.UserFirstName).HasColumnName("user_first_name");
            entity.Property(e => e.UserName).HasColumnName("user_name");
            entity.Property(e => e.UserPassword).HasColumnName("user_password");
            entity.Property(e => e.UserPatronymic).HasColumnName("user_patronymic");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.UserSurname).HasColumnName("user_surname");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("user_fk");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_role_pk");

            entity.ToTable("user_role");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });
        modelBuilder.HasSequence("task_seq");
        modelBuilder.HasSequence("task_status_seq");
        modelBuilder.HasSequence("user_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
