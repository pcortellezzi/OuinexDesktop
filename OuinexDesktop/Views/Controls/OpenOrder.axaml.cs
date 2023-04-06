using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using ScottPlot;
using ScottPlot.Avalonia;
using Avalonia.Xaml.Interactions.DragAndDrop;
using Avalonia.Controls.Documents;
using System;
using ScottPlot.Plottable;
using DynamicData;
using Avalonia.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using OuinexDesktop.ViewModels;

namespace OuinexDesktop.Views.Controls
{
    public partial class OpenOrder : UserControl
    {
        private  AvaPlot plot;
        private Crosshair crosshair;
        private double crossY;
        private TickerViewModel _ticker;
        public OpenOrder(TickerViewModel ticker)
        {
            InitializeComponent();

            _ticker = ticker;

            SetupChart();
            SetupeTPLogic();
            SetupeTSLogic();
            SetupContext(); 

            this.popup.Opened += (s, e) => this.popup.Height = this.FindAncestorOfType<ContainerWindow>()?.Height == null ? 400 : this.FindAncestorOfType<ContainerWindow>().Height;
        }


        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            this.FindAncestorOfType<ContainerWindow>()?.Close();
        }

        private void SetupChart()
        {
            plot = this.Find<AvaPlot>("avaPlot1");

            crosshair = avaPlot1.Plot.AddCrosshair(0, 0);
            crosshair.IgnoreAxisAuto = true;

            plot.PointerMoved += (o, e) =>
            {
                (double coordinateX, double coordinateY) = plot.GetMouseCoordinates();

                crosshair.X = coordinateX;
                crosshair.Y = coordinateY;

                crossY = coordinateY;

                avaPlot1.Refresh();
            };

            plot.Refresh();
        }

        private void SetupContext()
        {
            var menu = new ContextMenu();           

            var item1 = new MenuItem
            {
                Header = "Item 1"
            };
            item1.Click += (o, e) => slTextBox.Text = crossY.ToString();

            var item2 = new MenuItem
            {
                Header = "Item 2"
            };
            item2.Click += (o, e) => tpTextBox.Text = crossY.ToString();

            var item3 = new MenuItem
            {
                Header = "Item 3"
            };

            menu.Items = new[]
            {
                item1,
                item2,
                item3 
            };

            menu.MenuOpened += (s, e) => 
            {
                item1.Header = string.Format("Place stop loss at : {0}", crossY);
                item2.Header = string.Format("Place take profit at : {0}", crossY);
                item3.Header = string.Format("Enter price at : {0}", crossY);
            };

            plot.ContextMenu = menu;
        }


        private void SetupeTPLogic()
        {
            var tp = plot.Plot.AddHorizontalLine(0, System.Drawing.Color.LimeGreen, 2, label: "Take Profit");
            tp.DragEnabled = true;
            tp.IsVisible = false;
            tp.PositionLabel = true;

            tp.Dragged += (s, e) => tpTextBox.Text = tp.Y.ToString();
            tpTextBox.GotFocus += (s, e) =>
            {
                tp.IsVisible = true;

                if (string.IsNullOrEmpty(tpTextBox.Text))
                    tp.Y = 0;

                plot.Refresh();
            };

            tpTextBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(tpTextBox.Text))
                    return;

                tp.IsVisible = plot.IsPointerOver;

                plot.Refresh();
            };

            tpTextBox.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(slTextBox.Text))
                {
                    //could.IsVisible = false;
                    tp.IsVisible = false;
                }

                if (decimal.TryParse(tpTextBox.Text, out var price))
                {
                    tp.Y = (double)price;                    
                }
                if (string.IsNullOrEmpty(slTextBox.Text))
                {                   
                    tp.IsVisible = false;
                }

                plot.Refresh();
            };
        }

        private decimal _previousSl = 0;
        private void SetupeTSLogic()
        {
            var sl = plot.Plot.AddHorizontalLine(0, System.Drawing.Color.OrangeRed, 2, label: "Stop Loss");

            sl.DragEnabled = true;
            sl.IsVisible = false;
            sl.PositionLabel = true;

            sl.Dragged += (s, e) => slTextBox.Text = sl.Y.ToString();

            slTextBox.GotFocus += (s, e) =>
            {
                sl.IsVisible = true;

                if (string.IsNullOrEmpty(slTextBox.Text))
                {
                    sl.Y = 0;
                }

                plot.Refresh();
            };

            slTextBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(slTextBox.Text))
                    return;

                sl.IsVisible = plot.IsPointerOver;
                
                plot.Refresh();
            };

            slTextBox.TextChanged += (s, e) =>
            {
                if(string.IsNullOrEmpty(slTextBox.Text))
                {
                    sl.IsVisible = false;
                }

                if (decimal.TryParse(slTextBox.Text, out var price))
                {
                    sl.IsVisible = true;
                    sl.Y = (double)price;           
                }
                else
                {
                    sl.IsVisible = false;
                }

                plot.Refresh();
            };          
        }

        public async Task Populate()
        {
            await Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                var client = new Binance.Net.Clients.BinanceClient();

                var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(_ticker.Symbol.Name, Binance.Net.Enums.KlineInterval.OneHour, limit:100);

                if (request.Success)
                {
                    List<OHLC> prices = new List<OHLC>();

                    foreach (var data in request.Data)
                    {
                        prices.Add(new OHLC((double)data.OpenPrice,
                            (double)data.HighPrice,
                            (double)data.LowPrice,
                            (double)data.ClosePrice,
                            data.OpenTime,
                            //TODO : coonvert time frime to timespan
                            new TimeSpan(1, 0, 0)));
                    }

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        var test = plot.Plot.AddCandlesticks(prices.ToArray());
                        plot.Refresh();

                    }, DispatcherPriority.Background);
                }
            }));
        }
    }
}
