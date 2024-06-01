using System.Windows.Input;

namespace TouchGame
{
	public class TouchBehaviorViewModel : BaseViewModel
	{
        private readonly INavigation _navigation;

        private int _touches, _winners;
        private bool _isDebug;
        private Brush _background;
         
        public TouchBehaviorViewModel(INavigation navigation, int winners, Brush background)
        {
            _navigation = navigation;

            Background = background;
            Winners = winners;

            GoBackCommand = new Command(GoBack);
        }

        public ICommand GoBackCommand { get; }

        public new string Title => _touches + " touches";

        public bool IsDebug
        {
            get => _isDebug;
            set => SetProperty(ref _isDebug, value);
        }

        public int Touches
        {
            get => _touches;
            set => SetProperty(ref _touches, value, dependencies: nameof(Title));
        }

        public Brush Background
        {
            get => _background;
            set => SetProperty(ref _background, value);
        }

        public int Winners
        {
            get => _winners;
            set => SetProperty(ref _winners, value);
        }

        private async void GoBack()
        {
            await _navigation.PopModalAsync();
        }
    }
}

