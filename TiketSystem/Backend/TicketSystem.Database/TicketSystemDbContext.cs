using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TicketSystem.Database.Models;
using Task = TicketSystem.Database.Models.Task;
using TaskStatus = TicketSystem.Database.Models.TaskStatus;

namespace TicketSystem.Database;

public partial class TicketSystemDbContext : IdentityDbContext<User>
{


    public TicketSystemDbContext()
    {
        Database.EnsureCreated();
    }

    public TicketSystemDbContext(DbContextOptions<TicketSystemDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskStatus>().HasData(
            new TaskStatus[]
            {
                new TaskStatus { Id=1, Title= "В процессе", Priority= 1},
                new TaskStatus { Id=2, Title= "Отложено", Priority= 2},
                new TaskStatus { Id=3, Title= "Завершено", Priority= 3},
                new TaskStatus { Id=4, Title= "Отменено", Priority= 4},
     
            });
        base.OnModelCreating(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
