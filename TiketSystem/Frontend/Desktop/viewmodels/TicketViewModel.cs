using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Fizzler;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Events;
using Shared.ViewModels;
using TicketSystem.Backend.Controllers;
using TicketSystem.Database.Models;
using TicketSystem.Desktop.Events;
using MyTask = TicketSystem.Database.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace TicketSystem.Desktop.ViewModels
{
    public class TicketViewModel : BaseViewModel
    {

        private readonly IEventAggregator _eventAggregator;
        private const string url = "http://localhost:7253/api/Task/";

        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(url),
        };

        private readonly HubConnection _hubConnection;
        private ObservableCollection<MyTask> tasks = new();
        private LoginResponseViewModel _userModel;


        public ObservableCollection<MyTask> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged();
            }
        }

        #region commands

        public DelegateCommand<MyTask> CancelTaskCommand { get; private set; }
        public DelegateCommand<MyTask> EditTaskCommand { get; private set; }
        public DelegateCommand<MyTask> WaitTaskCommand { get; private set; }
        public DelegateCommand<MyTask> ConfirmTaskCommand { get; private set; }
        #endregion


        public TicketViewModel(LoginResponseViewModel userModel, IEventAggregator eventAggregator)
        {
            _userModel = userModel;
            _eventAggregator = eventAggregator; 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userModel.Token);
            CancelTaskCommand = new DelegateCommand<MyTask>(async (task) => await OnCancelTask(task));
            EditTaskCommand = new DelegateCommand<MyTask>( OnEditTask);
            ConfirmTaskCommand = new DelegateCommand<MyTask>( async (task) => await OnConfirmTask(task));
            WaitTaskCommand = new DelegateCommand<MyTask>(async (tasks) => await OnWaitTask(tasks));

            LoadTaskList();



            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:7253/ticketHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_userModel.Token);
                })
                .Build();

            Connect();
            _hubConnection.On<string>("connectionId", (connectionId) =>
            {
                userModel.ConnectionId = connectionId;
            });
            _hubConnection.On<MyTask>("newTask", (task) =>
            {
                Tasks.Add(task);
            });

        }

        private async Task OnWaitTask(MyTask task)
        {

            using StringContent jsonContent = new(

                JsonSerializer.Serialize(task),
                Encoding.UTF8,
                "application/json");
            try
            {
                
                using HttpResponseMessage response = await _httpClient.PatchAsync($"{url}{5}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    LoadTaskList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            LoadTaskList();
        }

        private async Task OnConfirmTask(MyTask task)
        {


            using StringContent jsonContent = new(

                JsonSerializer.Serialize(task),
                Encoding.UTF8,
                "application/json");
            try
            {
                using HttpResponseMessage response = await _httpClient.PatchAsync($"{url}{1}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    LoadTaskList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void OnEditTask(MyTask task)
        {
            _eventAggregator.GetEvent<AddEditTaskEvent>().Publish(new AddEditTaskEventParameters{LoginResponseViewModel = _userModel,Task = task});
        }

        private async Task OnCancelTask(MyTask task)
        {
            var test = JsonSerializer.Serialize(task);
            using StringContent jsonContent = new(

                JsonSerializer.Serialize(task),
                Encoding.UTF8,
                "application/json");
            try
            {
                using HttpResponseMessage response = await _httpClient.PatchAsync($"{url}{3}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    LoadTaskList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            LoadTaskList();
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

            }

        }

    }
}
