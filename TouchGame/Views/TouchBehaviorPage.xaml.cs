namespace TouchGame;

public partial class TouchBehaviorPage : ContentPage
{
	public TouchBehaviorPage()
	{
		InitializeComponent();
	}

    void RestartGame(object sender, EventArgs e)
    {
        touchBox.RestartGame();
    }
}
