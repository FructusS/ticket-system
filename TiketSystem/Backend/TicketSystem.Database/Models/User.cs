using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Database.Models;

public partial class User : IdentityUser
{
    public string? UserPatronymic { get; set; }

    public string? UserSurname { get; set; }

    public string? UserFirstName { get; set; }
    [JsonIgnore]
    public override int AccessFailedCount { get; set; }



    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

}
