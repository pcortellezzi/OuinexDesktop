using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;
using TestingApp.ViewModels;
using TestingApp.Views;

namespace TestingApp
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
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            Task.Run(async () => await Test());

            base.OnFrameworkInitializationCompleted();
        }

        private async Task Test()
        {
            var client = new OuinexDesktop.Exchanges.POCEx();
            await client.InitAsync();
        }
    }
}