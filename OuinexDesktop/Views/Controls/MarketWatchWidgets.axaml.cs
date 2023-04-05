using Avalonia.Controls;
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
    }
}
