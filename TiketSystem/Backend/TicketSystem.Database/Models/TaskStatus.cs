using System;
using System.Collections.Generic;

namespace TicketSystem.Database.Models;

public partial class TaskStatus
{
    public TaskStatus()
    {
        Tasks = new HashSet<Task>();
    }
    public int Id { get; set; }

    public string Title { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } 
}
