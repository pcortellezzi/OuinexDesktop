using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OuinexDesktop.Views.CustomControls
{
    public partial class AdvancedPriceBid : UserControl
    {
        public AdvancedPriceBid()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<object> PriceProperty = AvaloniaProperty.Register<AdvancedPriceBid, object>(nameof(Price));

        public object Price
        {
            get { return GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }
    }
}
