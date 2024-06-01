using System.Windows.Input;

namespace TouchGame
{
	public class WelcomeViewModel : ObservableObject
	{
		private readonly INavigation _navigation;

		private int _winnersCount;
        private Brush _selectedColor;

		public WelcomeViewModel(INavigation navigation)
		{
            _navigation = navigation;

            SelectWinnersCommand = new Command<int>(OnSelectWinners);
            SelectBackgroundCommand = new Command<Brush>(OnSelectBackground);
            StartGameCommand = new Command(OnStartGame);

            Winners = new List<int> { 1, 2, 3, 4 };

            BackgroundColors = ColorHelper.GetBackgroundBrushes();

            SelectedColor = BackgroundColors.First();
        }

        public ICommand SelectWinnersCommand { get; }
        public ICommand SelectBackgroundCommand { get; }
        public ICommand StartGameCommand { get; }

        public List<int> Winners { get; set; }

        public bool IsEnabled => WinnersCount > 0 && SelectedColor != null;

        public List<Brush> BackgroundColors { get; set; }

        public Brush SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value, dependencies: nameof(IsEnabled));
        }

        public int WinnersCount
        {
            get => _winnersCount;
            set => SetProperty(ref _winnersCount, value, dependencies: nameof(IsEnabled));
        }

        private void OnSelectWinners(int count)
        {
            WinnersCount = count;
        }

        private void OnSelectBackground(Brush brush)
        {
            SelectedColor = brush;
        }

        private async void OnStartGame()
		{
            if (!IsEnabled)
            {
                return;
            }

			var page = new TouchBehaviorPage();
			page.BindingContext = new TouchBehaviorViewModel(page.Navigation, WinnersCount, SelectedColor);

			await _navigation.PushModalAsync(page);
		}
	}
}

