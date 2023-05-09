using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ViewModels;

namespace Desktop;

public class MainViewModel : BaseViewModel
{
    HttpClient client;
    HttpClientHandler clientHandler;
    HubConnection hubConnection;
    private string title;
    private ObservableCollection<MyTask> tasks = new();
    public string Title
    {
        get { return title; }
        set
        {
            title = value;
            OnPropertyChanged("Title");
        }
    }

    public ObservableCollection<MyTask> Tasks
    {
        get { return tasks; }
        set
        {
            tasks = value;
            OnPropertyChanged("Tasks");
        }
    }

    public MainViewModel()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7253/ticketHub")
            .Build();

           
        hubConnection.Closed += async (error) =>
        {

            try
            {
                await Task.Delay(3000);
                await Connect();
            }
            catch
            {

            }
                   
              
       

        };


        hubConnection.On<MyTask>("newTask", (task) =>
        {
              
                Tasks.Add(task);
        });
    }

    public void LoadTaskList()
    {
        Tasks.Add(new MyTask
        {
            CreatedAt = DateTime.Now,
            Description = "132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbda132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmasmnbdmas",
            Title = "test1"
        });
        Tasks.Add(new MyTask
        {
            CreatedAt = DateTime.Now,
            Description = "132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbda132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmasmnbdmas",
            Title = "test2"
        });
        Tasks.Add(new MyTask
        {
            CreatedAt = DateTime.Now,
            Description = "132812312731238127jaksdshajkdhashbdans132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmasmbdanmsbdamnbsdnasbdamnbdmas",
            Title = "test3"
        });
        Tasks.Add(new MyTask
        {
            CreatedAt = DateTime.Now,
            Description = "132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbd132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmasamnbdmas",
            Title = "test4"
        });
    }
        public async Task Connect()
            {
                 

        try
        {
            await this.hubConnection.StartAsync();
            //  await GetMessages();
            await hubConnection.InvokeAsync("SendTask", new MyTask()
            {
                Cabinet = "123",
                CreatedAt = DateTime.Now,
                Title = "asd"
            });
        }
        catch(Exception ex)
        {
            SendLocalMessage(String.Empty, "не удалось подключиться \n" + ex.Message);
        }
        


    }

    private void SendLocalMessage(string empty, string ex)
    {
        Title = ex + empty;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}