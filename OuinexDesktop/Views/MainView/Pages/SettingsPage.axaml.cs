using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using SukiUI;

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
            SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Light);
            app.RequestedThemeVariant = ThemeVariant.Light;
            StockPlot.Charts.Helpers.PlotHelper.SetWhiteStyle();
        }
        else
        {
            SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Dark);
            app.RequestedThemeVariant = ThemeVariant.Dark;
            StockPlot.Charts.Helpers.PlotHelper.SetBlackStyle();
        }

        Statics.Theme = app.RequestedThemeVariant;
    }
}