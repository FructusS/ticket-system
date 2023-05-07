using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicketSystem.Backend.Models;

public class TaskStatus
{
    public int Id { get; set; }

    public string? Title { get; set; }
    [JsonIgnore]

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

}
