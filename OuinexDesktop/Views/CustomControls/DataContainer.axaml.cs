using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace OuinexDesktop.Views.CustomControls
{
    public partial class DataContainer : UserControl
    {
        public DataContainer()
        {
            InitializeComponent();
        }

        public static readonly StyledProperty<Geometry> PathProperty = AvaloniaProperty.Register<DataContainer, Geometry>(nameof(Path));

        public Geometry Path
        {
            get { return GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly StyledProperty<bool> IsPathVisibleProperty = AvaloniaProperty.Register<DataContainer, bool>(nameof(IsPathVisible));

        public bool IsPathVisible
        {
            get { return GetValue(IsPathVisibleProperty); }
            set { SetValue(IsPathVisibleProperty, value); }
        }

        public static readonly StyledProperty<bool> IsBusyProperty = AvaloniaProperty.Register<DataContainer, bool>(nameof(IsBusy));

        public bool IsBusy
        {
            get { return GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
    }
}
