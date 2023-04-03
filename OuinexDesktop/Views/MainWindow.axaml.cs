using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace OuinexDesktop.Views
{
    public partial class MainWindow : Window
    {
        private bool _isMenuOpen = true;


        public MainWindow()
        {
            InitializeComponent();
            _MenuToggleButton = this.FindControl<ToggleButton>("MenuToggleButton");
            _MenuItems = this.FindControl<StackPanel>("MenuItems");
            _PageContent = this.FindControl<ContentControl>("PageContent");
        }

        private ToggleButton _MenuToggleButton { get; set; }
        private StackPanel _MenuItems { get; }
        private ContentControl _PageContent { get; }

        private void Page1Button_Click(object sender, RoutedEventArgs e)
        {
           // PageContent.Content = new Page1();
        }

        private void Page2Button_Click(object sender, RoutedEventArgs e)
        {
           // PageContent.Content = new Page2();
        }

        private void Page3Button_Click(object sender, RoutedEventArgs e)
        {
         //   PageContent.Content = new Page3();
        }

        private void MenuToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMenuOpen)
            {
                _MenuItems.IsVisible = false;
                _isMenuOpen = false;
            }
            else
            {
                _MenuItems.IsVisible = true;
                _isMenuOpen = true;
            }
        }
    }
}