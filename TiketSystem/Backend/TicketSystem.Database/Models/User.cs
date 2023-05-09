using System;
using System.Collections.Generic;

namespace TicketSystem.Database.Models;

public partial class User
{
    public User()
    {
        Tasks = new List<Task>();
    }
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }

    public string? UserPatronymic { get; set; }

    public string? UserSurname { get; set; }

    public string? UserFirstName { get; set; }

    public int UserRoleId { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }

    public virtual UserRole UserRole { get; set; }
}
