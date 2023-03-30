using Avalonia.Controls;
using Avalonia.Interactivity;
using SukiUI.Controls;

namespace OuinexDesktop.Views.Controls
{
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            InteractiveContainer.CloseDialog();
        }
    }
}
