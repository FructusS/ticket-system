using Prism.Events;
using Shared.ViewModels;
using Shared.ViewModels.UserModels;
using TicketSystem.Backend.Controllers;
using TicketSystem.Database.Models;
using TicketSystem.Desktop.Events;
namespace TicketSystem.Desktop.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IEventAggregator _eventAggregator;
    private object _currentPage;
    public object CurrentPage
    {
        get { return _currentPage; }
        set
        {
            _currentPage = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel(IEventAggregator eventAggregator)
    {
        CurrentPage = new LoginViewModel(eventAggregator);
        _eventAggregator = eventAggregator; 
        eventAggregator.GetEvent<LoginEvent>().Subscribe(OnLogin);
        eventAggregator.GetEvent<AddEditTaskEvent>().Subscribe(OnAddEditTask);
    }

    private void OnAddEditTask(AddEditTaskEventParameters addEditTaskEventParameters)
    {
        CurrentPage = new AddEditTaskViewModel(addEditTaskEventParameters.Task, addEditTaskEventParameters.LoginResponseViewModel, _eventAggregator);
    }

    private void OnLogin(LoginResponseViewModel userModel)
    {
        CurrentPage = new TicketViewModel(userModel, _eventAggregator);
    }
}

