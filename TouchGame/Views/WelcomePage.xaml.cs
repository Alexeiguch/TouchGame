using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace TouchGame;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();

		BindingContext = new WelcomeViewModel(Navigation);

		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			this.Behaviors.Add(new StatusBarBehavior
			{
				StatusBarColor = Colors.Black,
				StatusBarStyle = StatusBarStyle.LightContent
			});
		}
	}
}
