using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicketSystem.Database.Models;

public partial class User
{
    public User()
    {
    }
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public string? UserPatronymic { get; set; }

    public string? UserSurname { get; set; }

    public string? UserFirstName { get; set; }

    public int UserRoleId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; } = new List<Task>();    
    

    public virtual UserRole UserRole { get; set; }
}
