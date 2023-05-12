using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class TaskController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;
        private readonly IHubContext<TicketHub> _hubContext;
        private readonly UserManager<User> _userManager;

        public TaskController(IHubContext<TicketHub> hubContext, UserManager<User> userManager, TicketSystemDbContext context)
        {
            _hubContext = hubContext;
            _userManager = userManager;
            _context = context;
        }


        [HttpGet("[action]/{username}")]
        public async Task<ActionResult> GetTasks(string username)
        {
            var tasks = new List<Task>();

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }
            var user = await _userManager.FindByNameAsync(username);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (user == null)
            {
                NotFound();
            }

            if (userRoles.Contains("Admin"))
            {
                tasks = _context.Tasks
                    .OrderByDescending(x => x.CreatedAt).ThenBy(x => x.TaskStatus.Priority)
                    .Reverse()
                    .ToList();
            }

            else
            {
                tasks = _context.Tasks
                   .Where(x => x.User.UserName == username)
                   .OrderByDescending(x => x.CreatedAt).ThenBy(x => x.TaskStatus.Priority)
                   .Reverse().Select(x => new Task
                   {
                       Cabinet = x.Cabinet,
                       CompletedAt = x.CompletedAt,
                       CreatedAt = x.CreatedAt,
                       Description = x.Description,
                       TaskStatusId = x.TaskStatusId,
                       Title = x.Title,
                       TaskStatus = x.TaskStatus,
                       Id = x.Id,
                       UserId = x.UserId,
                       User = null
                   })

                   .ToList();
            }

            return Ok(tasks);
        }




        [HttpPost]
        public async Task<ActionResult<Task>> Create([FromBody] Task task, string userId)
        {
            if (task is null)
                return BadRequest();

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group("admins").SendAsync("newTask", task);
            await _hubContext.Clients.Client(userId).SendAsync("newTask", task);

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

        [HttpPatch("{statusId}")]
        public async Task<ActionResult<Task>> Update([FromBody] Task task, int statusId)
        {

            var status = await _context.TaskStatuses.FindAsync(statusId);
            if (task == null || status == null) 
                return NotFound();

            task.TaskStatus = status;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}