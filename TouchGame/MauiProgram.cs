using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace TouchGame;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Lato-Black.ttf", "LatoBlack");
                fonts.AddFont("Lato-Bold.ttf", "LatoBold");
                fonts.AddFont("Lato-Light.ttf", "LatoLight");
                fonts.AddFont("Lato-Regular.ttf", "LatoRegular");
                fonts.AddFont("ionicons.ttf", "IonicIcons");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<IAudioService, AudioService>();

		return builder.Build();
	}
}

