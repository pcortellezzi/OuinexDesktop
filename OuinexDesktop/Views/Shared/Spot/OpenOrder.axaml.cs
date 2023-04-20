using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace OuinexDesktop.Views.Controls
{
    public partial class OpenOrder : UserControl
    {
        public OpenOrder()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            this.FindAncestorOfType<ContainerWindow>()?.Close();
        }        
    }    
}
