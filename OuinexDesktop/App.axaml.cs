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
                 var vm = new MainWindowViewModel();
                 var window = new MainWindow()
                 {
                     DataContext = vm
                 };


                 desktop.MainWindow = window;

                 window.Loaded +=  (async(sender, args) => 
                 {
                     ExchangesConnector.Instances["POC-Binance"].OnInit += () =>
                     {
                     };

                     await ExchangesConnector.Instances["POC-Binance"].InitAsync();
                 });

                Statics.MainWindow = window;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}