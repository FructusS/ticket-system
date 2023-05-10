using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using TicketSystem.Backend.Hubs;
using Task = TicketSystem.Database.Models.Task;
using TicketSystem.Database;
using TicketSystem.Shared.ViewModels;
using Shared.ViewModels;
using TicketSystem.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;
        private readonly IHubContext<TicketHub> _hubContext;

        public TasksController(TicketSystemDbContext ticketSystemDbContext, IHubContext<TicketHub> hubContext)
        {
            _context = ticketSystemDbContext;
            _hubContext = hubContext;
        }


        [HttpGet("[action]/{userName}")]
        public ActionResult GetTasks(string userName)
        {
            var tasks = new List<Task>();

            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest();
            }
            var user = _context.Users.FirstOrDefault(x => x.UserName == userName);

            if (user == null)
            {
                NotFound();
            }

            if (user.UserRole.Title == "Админ")
            {
                tasks = _context.Tasks
               .OrderByDescending(x => x.CreatedAt)
               .Reverse()
               .ToList();
            }
            else
            {
                tasks = _context.Tasks
               .Where(x => x.User.UserName == user.UserName)
               .OrderByDescending(x => x.CreatedAt)
               .Reverse()
               .ToList();
            }

            return Ok(tasks);
        }




        [HttpPost]
        public async Task<ActionResult<Task>> Create([FromBody] Task task)
        {
            if (task is null)
                return BadRequest();

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> Get(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();


            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Task>> Update(int id, string status)
        {
            var task = await _context.Tasks.FindAsync(id);
            task.Description = status;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("newTask", task);

            if (task == null)
                return NotFound();


            return Ok(task);
        }
    }
}