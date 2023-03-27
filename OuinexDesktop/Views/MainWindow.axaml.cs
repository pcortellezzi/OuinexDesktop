using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace OuinexDesktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ApplyTitleBar();
        }

        private void ToggleButton_OnIsCheckedChanged(object sender, RoutedEventArgs e)
        {
            var app = Application.Current;

            if (app is not null)
            {
                var theme = app.ActualThemeVariant;
                app.RequestedThemeVariant = theme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
                Statics.Theme = app.ActualThemeVariant;
            }
        }

        private void ApplyTitleBar()
        {
            this.titleBar.PointerPressed += (s, e) => this.BeginMoveDrag(e);
            this.titleBar.DoubleTapped += (s, e) => this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

            this.closeButton.PointerPressed += (s, e) => this.Close();
            this.Loaded += (s, e) => this.WindowState = WindowState.Maximized;
            this.minimizeButton.PointerPressed += (s, e) => this.WindowState = WindowState.Maximized;
            this.maximizeButton.PointerPressed += (s, e) => this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}