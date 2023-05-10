using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Desktop;

namespace TicketSystem.Desktop.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

    }
}
