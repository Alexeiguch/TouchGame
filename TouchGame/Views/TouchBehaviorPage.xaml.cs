using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace TouchGame;

public partial class TouchBehaviorPage : ContentPage
{
	public TouchBehaviorPage()
	{
		InitializeComponent();

        if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			this.Behaviors.Add(new StatusBarBehavior
			{
				// StatusBarColor = Colors.Black,
				StatusBarStyle = StatusBarStyle.DarkContent
			});
		}
	}

    void RestartGame(object sender, EventArgs e)
    {
        touchBox.RestartGame();
    }
}
