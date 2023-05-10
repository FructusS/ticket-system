using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.ViewModels;
using TicketSystem.Database;
using TicketSystem.Shared.ViewModels;

namespace TicketSystem.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly TicketSystemDbContext _context;

        public UsersController(TicketSystemDbContext ticketSystemDbContext)
        {
            _context = ticketSystemDbContext;
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult> Auth([FromBody] UserRequestModel loginModel)
        {
            if (loginModel is null)
            {
                return BadRequest();
            }
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.UserName == loginModel.UserName && x.UserPassword == loginModel.Password);
            if (user == null)
                return NotFound();


            return Ok(new UserResponseModel
            {
                UserName = user.UserName,
                UserRole = user.UserRole.Title,
                UserId = user.UserId
            });
        }
        [Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            var tasks = _context.Users.Find(userId);
            return Ok(tasks);
        }
    }
}
