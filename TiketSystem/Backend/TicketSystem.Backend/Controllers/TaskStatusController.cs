using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketSystem.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public TaskStatusController(TicketSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/<TaskStatusController>
        [HttpGet]
        public async Task<ActionResult<List<Database.Models.TaskStatus>>> Get()
        {
            return Ok(_context.TaskStatuses.ToList());
        }


        // POST api/<TaskStatusController>
        [HttpPost]
        public async Task<ActionResult> Post(string status)
        {
            var taskStatus = new Database.Models.TaskStatus
            {
                Title = status
            };
            _context.TaskStatuses.Add(taskStatus);
            await _context.SaveChangesAsync();
            return Ok(taskStatus);
        }

        // PUT api/<TaskStatusController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskStatusController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var taskStatus = await _context.TaskStatuses.FindAsync(id);
            _context.TaskStatuses.Remove(taskStatus);
            return Ok();  
        }
    }
}
