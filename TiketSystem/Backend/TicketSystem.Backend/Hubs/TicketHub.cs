using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Shared.ViewModels;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shared.ViewModels.UserModels;
using TicketSystem.Database;
using TicketSystem.Database.Models;
using TicketSystem.Shared.ViewModels;
using MyTask = TicketSystem.Database.Models.Task;
using Task = System.Threading.Tasks.Task;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Backend.Hubs
{
    [Authorize]
    public class TicketHub : Hub
    {
        private readonly TicketSystemDbContext _context;

        public TicketHub(TicketSystemDbContext context)
        {
            _context = context;
        }

        public async Task SendTask(MyTask task, string username)
        {
            if (task is not null)
            {
                // Send the message
                await Clients.Group("admins").SendAsync("newTask", task);
                await Clients.User(Context.ConnectionId).SendAsync("newTask", task);
            }
        }

        public async Task Join()
        {
            await Clients.Caller.SendAsync("connectionId", Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == Context.User.Identity.Name).FirstOrDefault();

                var userRole = _context.UserRoles.FirstOrDefault(x => x.UserId == user.Id);
                var role = _context.Roles.FirstOrDefault(x => x.Id == userRole.RoleId);
                if (role != null && role.Name == "Admin")
                {
                    Groups.AddToGroupAsync(Context.ConnectionId, "admins");
                }

                Join();
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnConnectedAsync();
        }
    }
}