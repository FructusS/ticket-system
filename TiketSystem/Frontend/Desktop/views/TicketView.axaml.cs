using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TicketSystem.Desktop.Views
{
    public partial class TicketView : UserControl
    {

        public TicketView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
