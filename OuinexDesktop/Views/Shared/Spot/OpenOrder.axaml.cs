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

            this.popup.Opened += (s, e) => this.popup.Height = this.FindAncestorOfType<ContainerWindow>()?.Height == null ? 400 : this.FindAncestorOfType<ContainerWindow>().Height;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            this.FindAncestorOfType<ContainerWindow>()?.Close();
        }        
    }    
}
