using Microsoft.AspNetCore.SignalR;
using TicketSystem.Database;
using Task = TicketSystem.Backend.Models.Task;

namespace TicketSystem.Backend.Hubs
{
    public class TicketHub : Hub
    {
        private TicketSystemDbContext _context;
        public TicketHub(TicketSystemDbContext context)
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task SendTask(Task task)
        {
            _context.Task.Add(task);
            _context.SaveChanges();
            await Clients.All.SendAsync("newTask", task);
            
        }
    }
}
