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
using TicketSystem.Backend.Controllers;
using Shared.ViewModels.UserModels;
using TicketSystem.Database.Models;
using Task = System.Threading.Tasks.Task;

namespace TicketSystem.Desktop.ViewModels
{


    public class LoginViewModel : BindableBase
    {

        private readonly IEventAggregator _eventAggregator;

        private const string url = "http://localhost:7253/api/User";
        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(url),
        };


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
            set => SetProperty(ref _errorText, value);
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
     
            _eventAggregator = eventAggregator;
         //   LoginCommand = new DelegateCommand(async () => await OnLogin(), CanSubmit).ObservesProperty(() => IsEnabled); 
            LoginCommand = new DelegateCommand(async () => await OnLogin()); 
        }


        bool CanSubmit()
        {
            return IsEnabled = !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        private async Task OnLogin()
        {


            using StringContent jsonContent = new(

            JsonSerializer.Serialize(new LoginRequestViewModel
            {
                Username = "admin",
                Password = "admin"
            }),
            Encoding.UTF8,
            "application/json");
            try
            {
                using HttpResponseMessage response = await _httpClient.PostAsync($"{url}/Login", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<LoginResponseViewModel>();
                    if (user == null)
                    {
                        ErrorText = "Неверный логин или пароль";
                        return;
                    }
                 
                    _eventAggregator.GetEvent<LoginEvent>().Publish(user);
                }
                else
                {
                    ErrorText = "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {

                ErrorText = ex.Message;
            }


        }
    }

}
