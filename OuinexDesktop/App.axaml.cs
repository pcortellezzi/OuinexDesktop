using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using OuinexDesktop.ViewModels;
using OuinexDesktop.Views;

namespace OuinexDesktop
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            RequestedThemeVariant = ThemeVariant.Dark;
            Statics.Theme = ThemeVariant.Dark;
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

            SetupDefaultFontSize();

            base.OnFrameworkInitializationCompleted();
        }

        private void SetupDefaultFontSize()
        {
            IResourceDictionary resources = Current.Resources;

            resources["ButtonDefaultFontSize"] = 11;
            resources["DefaultFontSize"] = 11;
            resources["TextBlockFontSize"] = 11;
            resources["CheckboxFontSize"] = 11;
            resources["CheckboxFontSize"] = 11;
        }
    }
}