using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicketSystem.Backend.Models;

public class Task
{
    public string? Description { get; set; }

    public int? TaskStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    [JsonPropertyName("cabinet")]
    public string? Cabinet { get; set; }

    public string? Title { get; set; }

    public int Id { get; set; }
    [JsonIgnore]

    public virtual TaskStatus? TaskStatus { get; set; }

}
