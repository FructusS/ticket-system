using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicketSystem.Database.Models;

public partial class TaskStatus
{
    public TaskStatus()
    {
    }
    public int Id { get; set; }

    public string Title { get; set; }
    public int Priority { get; set; }
    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; }  = new List<Task>();    
}
