using ReactiveUI;

namespace OuinexPro.ViewModels
{
    public class OrderBookItem : ViewModelBase
    {
        private decimal _price, _volume;
        private int _percent;

        public decimal Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value, nameof(Price));
        }

        public decimal Volume
        {
            get => _volume;
            set => this.RaiseAndSetIfChanged(ref _volume, value, nameof(Volume));
        }

        public int Percent
        {
            get => _percent * 2;
            set => this.RaiseAndSetIfChanged(ref _percent, value, nameof(Percent));
        }
    }
}