using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using ViewModels;

namespace TicketSystem.Desktop.viewmodels
{
    
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(OnLogin);

        }

        private void OnLogin()
        {
            
        }

        public string Login { get; set; }
        private string _login { get; set; }
        
        
        public string Password { get; set; }
        private string _password { get; set; }
        
        public DelegateCommand LoginCommand { get;  }
    }
}
