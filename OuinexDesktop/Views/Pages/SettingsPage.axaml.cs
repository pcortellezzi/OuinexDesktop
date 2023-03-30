using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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
        if((sender as ComboBox).SelectedIndex == 0)
        {
            SukiUI.ColorTheme.LoadLightTheme(Application.Current);
        }
        else
        {
            SukiUI.ColorTheme.LoadDarkTheme(Application.Current);
        }
    }
}