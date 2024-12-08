using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OuinexDesktop.Views.Controls;
using System.Collections;

namespace OuinexDesktop.Views.Pages;

public partial class MarketsPage : UserControl
{
    public MarketsPage()
    {
        InitializeComponent(); 
    }

    private void SearchBox_TextChanging(object? sender, TextChangedEventArgs e)
    {
        string searchText = (sender as TextBox).Text.ToLower();

        var test = this.FindControl<TreeView>("treeView");
        TraverseNodes(test.Items, searchText);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }


    private void TraverseNodes(IEnumerable items, string searchText)
    {

        foreach (var item in items)
        {
            var node = (TreeViewItem)item;
            if (node.Header.ToString().ToLower().Contains(searchText))
            {
                // The node matches the search text, so make it visible and expand its parent nodes
                node.IsExpanded = true;
                node.IsVisible = true;
                ExpandParentNodes(node);
            }
            else
            {
                // The node doesn't match the search text, so hide it
                node.IsExpanded = false;
                node.IsVisible = false;
            }

            TraverseNodes(node.Items, searchText);
        }
    }

    private void ExpandParentNodes(TreeViewItem node)
    {
        // Recursively expand all parent nodes of the specified node
        var parent = node.Parent as TreeViewItem;
        if (parent != null)
        {
            parent.IsExpanded = true;
            parent.IsVisible = true;
            ExpandParentNodes(parent);
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
       
    }
}