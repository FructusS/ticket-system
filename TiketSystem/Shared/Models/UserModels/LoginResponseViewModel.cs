namespace TicketSystem.Backend.Controllers;

public class LoginResponseViewModel
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public string UserRole { get; set; }
    public string UserId { get; set; }
    public string? ConnectionId { get; set; }
}