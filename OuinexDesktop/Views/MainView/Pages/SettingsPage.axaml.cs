using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using DynamicData.Kernel;

namespace OuinexDesktop.Views.Pages;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var app = Application.Current;

        if ((sender as ComboBox).SelectedIndex == 0)
        {
            SukiUI.ColorTheme.LoadLightTheme(Application.Current);
            app.RequestedThemeVariant = ThemeVariant.Light;
        }
        else
        {
            SukiUI.ColorTheme.LoadDarkTheme(Application.Current);
            app.RequestedThemeVariant = ThemeVariant.Dark;
        }

    }
}