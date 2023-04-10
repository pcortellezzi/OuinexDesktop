using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OuinexDesktop.Views.CustomControls
{
    public partial class TitledControl : UserControl
    {
        public TitledControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<string> TitleProperty =
   AvaloniaProperty.Register<TitledControl, string>(nameof(Title));

        public static readonly StyledProperty<UserControl> CustomBarContentProperty =
            AvaloniaProperty.Register<TitledControl, UserControl>(nameof(CustomBarContent));

        public string Title
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public UserControl CustomBarContent
        {
            get { return GetValue(CustomBarContentProperty); }
            set { SetValue(CustomBarContentProperty, value); }
        }
    }
}
