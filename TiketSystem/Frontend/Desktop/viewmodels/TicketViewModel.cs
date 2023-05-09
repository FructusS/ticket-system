using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using ViewModels;
using MyTask = TicketSystem.Database.Models.Task;
namespace TicketSystem.Desktop.viewmodels
{
    public class TicketViewModel : BaseViewModel
    {
        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri($"http://localhost:7253/api/Tasks"),
        };
        HttpClient client;
        HttpClientHandler clientHandler;
        HubConnection hubConnection;
        private ObservableCollection<MyTask> tasks = new();
        private string title { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public DelegateCommand LoginCommand { get; }
        public DelegateCommand TestCommand { get; }
        public ObservableCollection<MyTask> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged("Tasks");
            }
        }

        public TicketViewModel()
        {
            // LoadTaskList();

         //   LoginCommand = new DelegateCommand(OnLogin);
        
            hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:7253/ticketHub")
                .Build();

            Connect();
            LoadTaskList();
            hubConnection.On<MyTask>("newTask", (task) =>
            {
                Tasks.Add(task);
            });
        }

        private void LoadTaskList()
        {
           // Tasks = 
        }
        //private async void OnSendTaskClick()
        //{
        //    var task = new MyTask
        //    {
        //        CreatedAt = DateTime.UtcNow,
        //        Description =
        //            "132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbda132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmas132812312731238127jaksdshajkdhashbdansmbdanmsbdamnbsdnasbdamnbdmasmnbdmas",
        //        Title = "test1",
        //        UserId = 1,
        //        TaskStatusId = 1
        //    };
        //    await hubConnection.InvokeAsync("SendTask", task);

        //}


       
        public async Task Connect()
        {

            try
            {
                await this.hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                SendLocalMessage(String.Empty, "не удалось подключиться \n" + ex.Message);
            }

        }

        private void SendLocalMessage(string empty, string ex)
        {
            title = empty + ex;
        }
    }
}
