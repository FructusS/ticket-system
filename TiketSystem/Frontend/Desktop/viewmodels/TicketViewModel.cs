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
using Prism.Events;
using Shared.ViewModels;
using MyTask = TicketSystem.Database.Models.Task;
namespace TicketSystem.Desktop.ViewModels
{
    public class TicketViewModel : BaseViewModel
    {

        private readonly IEventAggregator _eventAggregator;


        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri($"http://localhost:7253/api/Tasks/"),
        };

        private readonly HubConnection _hubConnection;
        private ObservableCollection<MyTask> tasks = new();
        private UserResponseModel _userModel;

        private bool _isAdmin { get; set; }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }

        }

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
        public DelegateCommand SendTaskCommand { get; }
        public DelegateCommand AdminCommand { get; }
        public ObservableCollection<MyTask> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged("Tasks");
            }
        }

        public TicketViewModel(UserResponseModel userModel)
        {

            _userModel = userModel;


            LoadTaskList();



            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:7253/ticketHub")
                .Build();

            Connect();
            _hubConnection.On<MyTask>("newTask", (task) =>
            {
                Tasks.Add(task);
            });
            IsAdmin = _userModel.UserRole == "Админ";

        }



        private async Task LoadTaskList()
        {
            var tasks = await _httpClient.GetFromJsonAsync<List<MyTask>>($"GetTasks/{_userModel.UserName}");
            Tasks = new ObservableCollection<MyTask>(tasks);
        }

        public async Task Connect()
        {

            try
            {
                await this._hubConnection.StartAsync();
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
