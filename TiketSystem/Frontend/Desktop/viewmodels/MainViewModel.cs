using System.Collections.ObjectModel;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using ViewModels;
using MyTask = TicketSystem.Database.Models.Task;
namespace TicketSystem.Desktop.viewmodels;

public class MainViewModel : BaseViewModel
{
    public object CurrentPage { get; set; }


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
        CurrentPage = new TicketViewModel();

    }
}