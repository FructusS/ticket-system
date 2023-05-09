using AutoMapper;
using Chat.Web.ViewModels;
using Microsoft.AspNetCore.SignalR;
using TicketSystem.Database;
using TicketSystem.Database.Models;
using MyTask = TicketSystem.Database.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace TicketSystem.Backend.Hubs
{
    public class TicketHub : Hub
    { 
        public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();

        private readonly TicketSystemDbContext _context;
        private readonly IMapper _mapper;
        public TicketHub(TicketSystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task SendTask(MyTask task)
        {


            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            await Clients.Caller.SendAsync("newTask", task);
            await Clients.Group("admins").SendAsync("newTask", task);
            

        }

        public async Task Join(string username)
        {
            try
            {
                var user = _Connections.Where(u => u.UserName == username).FirstOrDefault();
                if (user != null && user.UserRole == "Админ")
                {
              
                    await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
                 
                }

            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == "admin").FirstOrDefault();

                var userViewModel = _mapper.Map<User, UserViewModel>(user);
                if (!_Connections.Any(u => u.UserName == user.UserName))
                { 
                    
                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(userViewModel.UserName, Context.ConnectionId);
                }

                
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var user = _context.Users.Where(u => u.UserName == "admin").First();
                var userViewModel = _mapper.Map<User, UserViewModel>(user);
                _Connections.Remove(userViewModel);


                // Remove mapping
                _ConnectionsMap.Remove(userViewModel.UserName);
            }
            catch (Exception ex)
            {
                Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }



    }
}
