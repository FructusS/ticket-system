using Prism.Events;
using Shared.ViewModels;
using TicketSystem.Backend.Controllers;

namespace TicketSystem.Desktop.Events
{
    public class LoginEvent : PubSubEvent<LoginResponseViewModel>
    {
    }
}