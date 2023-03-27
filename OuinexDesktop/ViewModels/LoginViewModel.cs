using ReactiveUI;
using System.Windows.Input;

namespace OuinexDesktop.ViewModels
{
    public delegate void OnLoggedEventHandler();

    public class LoginViewModel : ViewModelBase
    {
        public event OnLoggedEventHandler OnLogged;

        private string _userName = string.Empty;
        private string _password = string.Empty;
        private bool _logged = true;

        public LoginViewModel()
        {
            this.Login = ReactiveCommand.Create(() =>
            {
                SuccesfulLogin = false;
                this.OnLogged.Invoke();
            });
        }

        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value, nameof(UserName));
        }

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value, nameof(Password));
        }

        public bool SuccesfulLogin
        {
            get => _logged;
            set => this.RaiseAndSetIfChanged(ref _logged, value, nameof(SuccesfulLogin));
        }

        public ICommand Login { get; private set; }
    }
}
