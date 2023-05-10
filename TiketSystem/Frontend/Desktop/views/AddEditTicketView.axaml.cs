using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TicketSystem.Desktop.Views;

public partial class AddEditTicketView : UserControl
{
    public AddEditTicketView()
    {
        InitializeComponent();
       // SendTaskCommand = new DelegateCommand(OnSendTaskClick);

        //private async void OnSendTaskClick()
        //{
        //    var task = new MyTask
        //    {
        //        CreatedAt = DateTime.UtcNow,
        //        Description =
        //            "132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbda132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmasmnbdmas",
        //        Title = "test1",
        //        UserId = _userModel.UserId,
        //        TaskStatusId = 1
        //    };
        //    await _hubConnection.InvokeAsync("SendTask", task);

        //}


    }
}