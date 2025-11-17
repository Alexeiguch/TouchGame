namespace TouchGame;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		// MainPage = new WelcomePage();
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		return new Window(new WelcomePage());
	}
}

