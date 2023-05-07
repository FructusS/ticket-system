using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using TicketSystem.Backend.Hubs;
using TicketSystem.Backend.Models;
using TicketSystem.Database;

namespace TicketSystem.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;
        private readonly IHubContext<TicketHub> _hubContext;
        public TasksController(TicketSystemDbContext ticketSystemDbContext, IHubContext<TicketHub> hubContext) {
            _context = ticketSystemDbContext;
            _hubContext = hubContext;
        }


        [HttpGet("[action]/{countTask}")]
        public IActionResult GetTasks(int countTask)
        {
            var tasks = _context.Task.Take(countTask).ToList();
            return Ok(tasks);

        }
        [HttpPost]
        public async Task<ActionResult<Models.Task>> Create(Models.Task task)
        {
        
            if (task == null)
                return BadRequest();

            _context.Task.Add(task);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("newTask", task);

            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> Get(int id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task == null)
                return NotFound();

            
            return Ok(task);
        }

    }
}
