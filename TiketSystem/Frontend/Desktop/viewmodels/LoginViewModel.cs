using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using TicketSystem.Shared.ViewModels;
using TicketSystem.Desktop.Events;
using System.Reflection.Metadata;
using Prism.Mvvm;
using System.Net.Http.Json;
using Shared.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;

namespace TicketSystem.Desktop.ViewModels
{


    public class LoginViewModel : BindableBase
    {

        private readonly IEventAggregator _eventAggregator;

        private const string url = "http://localhost:7253/api/Users";
        private static HttpClient httpClient = new()
        {
            BaseAddress = new Uri(url),
        };

        private readonly HubConnection _hubConnection;

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
            }
        }


        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                SetProperty(ref _errorText, value);
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {

                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginCommand { get; private set; }

        public LoginViewModel(IEventAggregator eventAggregator)
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:7253/ticketHub")
            .Build();
            _eventAggregator = eventAggregator;
            LoginCommand = new DelegateCommand(async () => await OnLogin(), CanSubmit).ObservesProperty(() => IsEnabled); ;
        }


        bool CanSubmit()
        {
            return IsEnabled = !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        private async Task OnLogin()
        {


            using StringContent jsonContent = new(

            JsonSerializer.Serialize(new UserRequestModel
            {
                UserName = UserName,
                Password = Password
            }),
            Encoding.UTF8,
            "application/json");
            using HttpResponseMessage response = await httpClient.PostAsync(
     $"{url}/login",
     jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserResponseModel>();
                if (user == null)
                {
                    ErrorText = "Неверный логин или пароль";
                    return;
                }
                if (user.UserRole == "Админ")
                {

                    _hubConnection.InvokeAsync("Join", user.UserName);

                }
                _eventAggregator.GetEvent<LoginEvent>().Publish(user);
            }
            else
            {
                ErrorText = "Неверный логин или пароль";
            }
        }
    }

}
