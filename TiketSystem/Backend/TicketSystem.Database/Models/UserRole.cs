using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicketSystem.Database.Models;

public partial class UserRole
{
    public UserRole()
    {
    }
    public int Id { get; set; }

    public string? Title { get; set; }
    [JsonIgnore]

    public virtual ICollection<User> Users { get;  } = new List<User>();    
}
