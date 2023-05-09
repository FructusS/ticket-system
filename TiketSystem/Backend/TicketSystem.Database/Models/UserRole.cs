﻿using System;
using System.Collections.Generic;

namespace TicketSystem.Database.Models;

public partial class UserRole
{
    public UserRole()
    {
        Users = new HashSet<User>();
    }
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<User> Users { get; set; }
}
