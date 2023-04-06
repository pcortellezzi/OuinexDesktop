using Avalonia.Controls;
using Avalonia.Interactivity;
using ScottPlot;
using ScottPlot.Avalonia;

namespace OuinexDesktop.Views.Controls
{
    public partial class OpenOrder : UserControl
    {
        private  AvaPlot plot;
        public OpenOrder()
        {
            InitializeComponent();
            plot = this.Find<AvaPlot>("avaPlot1");

            SetupeTPLogic();
            SetupeTSLogic();

            plot.Plot.AddOHLCs(DataGen.RandomStockPrices(new System.Random(), 100, startingPrice:27000));
          
            plot.Refresh();
        }

        private void Tp_Dragged(object? sender, System.EventArgs e)
        {
            
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            (this.Parent as Window).Close();
        }

        private void SetupeTPLogic()
        {
            var tp = plot.Plot.AddHorizontalLine(27100, System.Drawing.Color.LimeGreen, 2, label: "Take Profit");
            tp.DragEnabled = true;
            tp.IsVisible = false;
            tp.PositionLabel = true;

            tp.Dragged += (s, e) => tpTextBox.Text = tp.Y.ToString();
            tpTextBox.GotFocus += (s, e) =>
            {
                tp.IsVisible = true;

                if (string.IsNullOrEmpty(tpTextBox.Text))
                    tp.Y = 27000;

                plot.Refresh();
            };

            tpTextBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(tpTextBox.Text))
                    return;

                tp.IsVisible = plot.IsPointerOver;

                plot.Refresh();
            };

            tpTextBox.TextChanging += (s, e) =>
            {
                if (decimal.TryParse(tpTextBox.Text, out var price))
                {
                    tp.Y = (double)price;
                    plot.Refresh();
                }
            };
        }

        private void SetupeTSLogic()
        {
            var sl = plot.Plot.AddHorizontalLine(27000, System.Drawing.Color.OrangeRed, 2, label: "Stop Loss");
            var could = plot.Plot.AddVerticalSpan(27000, 27000, System.Drawing.Color.FromArgb(30, System.Drawing.Color.OrangeRed),  label: "Stop Loss");

            sl.DragEnabled = true;
            sl.IsVisible = false;
            sl.PositionLabel = true;

            could.IsVisible = false;
            sl.Dragged += (s, e) => slTextBox.Text = sl.Y.ToString();

            slTextBox.GotFocus += (s, e) =>
            {
                sl.IsVisible = true;
                could.IsVisible = true;

                if (string.IsNullOrEmpty(slTextBox.Text))
                {
                    sl.Y = 27000;
                    could.Y1 = 27000;
                    could.Y2 = 27000;
                }

                plot.Refresh();
            };

            slTextBox.LostFocus += (s, e) =>
            {
                if (!string.IsNullOrEmpty(slTextBox.Text))
                    return;

                sl.IsVisible = plot.IsPointerOver;
                could.IsVisible = plot.IsPointerOver;
                
                plot.Refresh();
            };

            slTextBox.TextChanging += (s, e) =>
            {
                if (decimal.TryParse(slTextBox.Text, out var price))
                {
                    sl.Y = (double)price;
                    could.Y2 = (double)price;
                    could.Y1 = (double)27000;

                    plot.Refresh();
                }
            };
        }
    }
}
