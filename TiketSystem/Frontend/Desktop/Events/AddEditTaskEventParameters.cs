using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSystem.Backend.Controllers;
using Task = TicketSystem.Database.Models.Task;

namespace TicketSystem.Desktop.Events
{
    public class AddEditTaskEventParameters
    {
        public Task Task { get; set; }
        public LoginResponseViewModel LoginResponseViewModel { get; set; }
    }
}
