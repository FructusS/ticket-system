using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Prism.Events;
using TicketSystem.Backend.Controllers;
using TicketSystem.Database.Models;
using TicketSystem.Desktop.Events;
using MyTask = TicketSystem.Database.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace TicketSystem.Desktop.ViewModels
{
    public class AddEditTaskViewModel : BaseViewModel
    {
        private const string url = "http://localhost:7253/api/Task/";

        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(url),
        };
        private bool _isSelected;
        private MyTask _task;
        private LoginResponseViewModel _loginResponse;
        private readonly IEventAggregator _eventAggregator;

        public MyTask Task
        {
            get { return _task; }
            set { _task = value; OnPropertyChanged();}
        }

        public DelegateCommand SaveTaskCommand { get; private set; }

        public AddEditTaskViewModel(MyTask task, LoginResponseViewModel loginResponseViewModel,
            IEventAggregator eventAggregator)
        {
            _task = task;
            _loginResponse = loginResponseViewModel;
            _eventAggregator = eventAggregator;
            SaveTaskCommand = new DelegateCommand(async () => await OnSaveTask());
        }

        private async Task OnSaveTask()
        {
            using StringContent jsonContent = new(

                JsonSerializer.Serialize(_task),
                Encoding.UTF8,
                "application/json");
            try
            {

                using HttpResponseMessage response = await _httpClient.PatchAsync($"{url}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    _eventAggregator.GetEvent<LoginEvent>().Publish(_loginResponse);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }


    }
}
