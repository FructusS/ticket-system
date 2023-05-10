using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TicketSystem.Database.Models;

public partial class Task
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? Description { get; set; }
    [DefaultValue(1)]
    public int? TaskStatusId { get; set; }

    public string? Title { get; set; }

    public string? Cabinet { get; set; }

    public int? UserId { get; set; }

    public virtual TaskStatus TaskStatus { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
