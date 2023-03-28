using Avalonia.Controls;

namespace OuinexPro.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ApplyTitleBar();
        }

        private void ApplyTitleBar()
        {
            this.titleBar.PointerPressed += (s, e) => this.BeginMoveDrag(e);
            this.titleBar.DoubleTapped += (s, e) => this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            this.closeButton.PointerPressed += (s, e) => this.Close();
            this.minimizeButton.PointerPressed += (s, e) => this.WindowState = WindowState.Maximized;
            this.maximizeButton.PointerPressed += (s, e) => this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}