using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shared.ViewModels.UserModels;
using TicketSystem.Database;
using TicketSystem.Database.Models;
using TicketSystem.Shared.ViewModels;
using Task = System.Threading.Tasks.Task;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TicketSystem.Backend.cfg;

namespace TicketSystem.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager, IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
            _userManager = userManager;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model is null)
            {
                return BadRequest();
            }

            var user = new User { UserName = model.Username };
            var createdResult = await _userManager.CreateAsync(user, model.Password);

            if (createdResult.Succeeded)
            {
                return Ok(user.Id);
            }

            return BadRequest(createdResult.Errors);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            if (model is null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return NotFound("user not found");
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new LoginResponseViewModel
                {
                    Token = token,
                    UserName = user.UserName,
                    UserRole = userRoles.FirstOrDefault(x=> x == UserRoles.Admin),
                    UserId = user.Id,
                    ConnectionId = null

                });
            }
            return Unauthorized();

        }


        //[HttpPost("[action]")]
        //public async Task<IActionResult> AddRole(string id, string role)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    var createdResult = await _userManager.AddToRoleAsync(user, role);

        //    if (createdResult.Succeeded)
        //    {
        //        return Ok(user.Id);
        //    }
        //    foreach (var error in createdResult.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return BadRequest(ModelState);
        //}

        private string GetToken(List<Claim> authClaims)
        {

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.Now.AddHours(_jwtConfig.AccessTokenLifeTime),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_jwtConfig.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}