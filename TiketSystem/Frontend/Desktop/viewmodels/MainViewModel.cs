using Prism.Events;
using Shared.ViewModels;
using TicketSystem.Desktop.Events;
namespace TicketSystem.Desktop.ViewModels;

public class MainViewModel : BaseViewModel
{
    private object _currentPage;
    public object CurrentPage
    {
        get { return _currentPage; }
        set
        {
            _currentPage = value;
            OnPropertyChanged("CurrentPage");
        }
    }

    public MainViewModel(IEventAggregator eventAggregator)
    {
        CurrentPage = new LoginViewModel(eventAggregator);

        eventAggregator.GetEvent<LoginEvent>().Subscribe(OnLogin);
    }

    private void OnLogin(UserResponseModel userModel)
    {
        CurrentPage = new TicketViewModel(userModel);
    }
}