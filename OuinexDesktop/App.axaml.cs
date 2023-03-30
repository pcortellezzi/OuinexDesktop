using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using OuinexDesktop.ViewModels;
using OuinexDesktop.Views;
using OuinexDesktop.Views.Controls;
using SukiUI.Controls;

namespace OuinexDesktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var window = new FreshWindow()
                {

                };
                desktop.MainWindow = window;
                window.Loaded += ((sender, args) => InteractiveContainer.ShowDialog(new LoginControl()));
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}