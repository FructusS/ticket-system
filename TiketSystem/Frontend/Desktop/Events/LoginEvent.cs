using Prism.Events;
using Shared.ViewModels;

namespace TicketSystem.Desktop.Events
{
    public class LoginEvent : PubSubEvent<UserResponseModel>
    {
    }
}