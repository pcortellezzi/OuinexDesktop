using ReactiveUI;

namespace OuinexDesktop.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _isEmptyOfData = false;
        private bool _isBusy = false;

        public bool IsEmptyOfData
        {
            get => _isEmptyOfData;
            set => this.RaiseAndSetIfChanged(ref _isEmptyOfData, value, nameof(IsEmptyOfData)); 
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isEmptyOfData, value, nameof(IsBusy));
        }
    }
}