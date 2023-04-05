using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using OuinexDesktop.Models;
using OuinexDesktop.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OuinexDesktop.Views.Controls
{
    public partial class MarketWatchWidgets : UserControl
    {
        private List<Symbol> _symbolList;
        public MarketWatchWidgets()
        {
            InitializeComponent();
            this.Initialized += MarketWatchWidgets_Initialized;
            ExchangesConnector.Instances.First().Value.OnInit += Value_OnInit;
        }

        private void Value_OnInit()
        {
            _symbolList = ExchangesConnector.Instances.First().Value.Symbols.ToList();
            this.allSymbolsGrid.Items = _symbolList;
        }


        private void MarketWatchWidgets_Initialized(object? sender, System.EventArgs e)
        {
            this.searchSymbol.TextChanging += SearchSymbol_TextChanging;
            this.ratesGrid.DoubleTapped += RatesGrid_DoubleTapped;
            this.ratesGrid.LoadingRow += RatesGrid_LoadingRow;
        }
         

        private void RatesGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
          // this.ratesGrid.Height = 
        }

        private void RatesGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            if (ratesGrid.SelectedItem != null)
                (ratesGrid.SelectedItem as TickerViewModel).OpenATicketCommand.Execute(null);
        }

        private void SearchSymbol_TextChanging(object? sender, TextChangingEventArgs e)
        {
            if (this.searchSymbol.Text != string.Empty)
            {
                string searchText = searchSymbol.Text.ToLower();

                // filter the ObservableCollection based on the search text
                var filteredData = _symbolList.Where(d => d.FullName.ToLower().Contains(searchText)).ToList();

                // set the Items property of the DataGrid to the filtered ObservableCollection
                this.allSymbolsGrid.Items = filteredData;
            }
            else
            {
                this.allSymbolsGrid.Items = _symbolList;
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            var topLevel = TopLevel.GetTopLevel(this);
            var _manager = new WindowNotificationManager(topLevel) { MaxItems = 3 };

            (DataContext as MarketWatchViewModel)._manager = _manager;
        }
    }
}
