using OuinexPro.Views;
using ReactiveUI;
using OuinexPro.Views.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;

namespace OuinexPro.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.LoginMVVM.OnLogged += LoginMVVM_OnLogged;

            this.OpenMarketWatchWindow = ReactiveCommand.Create(() =>
            {
                var window = new ContainerWindow()
                {
                    
                    Title = "market watch"
                };

                window.mainContainer.Children.Add(new MarketWatchWidgets()
                {
                    DataContext = this.MarketWatchMVVM
                });
                window.Show();
            });
        }

        private void LoginMVVM_OnLogged()
        {
            Task.Run(async () => await this.MarketWatchMVVM.InitStream());
        }

        private void CreateAControl()
        {
            var control = new Control();
            
        }

        public LoginViewModel LoginMVVM { get; } = new LoginViewModel();

        public MarketWatchViewModel MarketWatchMVVM { get; } = new MarketWatchViewModel();

        public ICommand OpenMarketWatchWindow { get; }
    }
}