using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OuinexDesktop.ViewModels;

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
                 /*var vm = new MainWindowViewModel();
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

                Statics.MainWindow = window;*/

              desktop.MainWindow = new Window1();
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}