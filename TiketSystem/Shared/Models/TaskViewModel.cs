using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Database.Models;
using TaskStatus = TicketSystem.Database.Models.TaskStatus;

namespace Shared.ViewModels
{
    public class TaskViewModel
    {

        public DateTime? CompletedAt { get; set; }

        public string? Description { get; set; }
        [DefaultValue(1)]
        public int? TaskStatusId { get; set; }

        public string? Title { get; set; }

        public string? Cabinet { get; set; }
        public TaskStatus TaskStatus { get; set; } = null!;
        public User User { get; set; } = null!;

    }
}
