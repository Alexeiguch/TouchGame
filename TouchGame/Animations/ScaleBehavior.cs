using CommunityToolkit.Maui.Behaviors;

namespace TouchGame
{
	public class ScaleBehavior
	{
		public static AnimationBehavior UpDownScaleBehavior(Easing easing, uint length, double upScale, uint? downLength = null, double? downScale = null)
		{
            return new AnimationBehavior
            {
                AnimationType = new UpDownScaleAnimation
                {
                    Easing = easing,
                    Length = length,
                    DownLength = downLength,
                    UpScale = upScale,
                    DownScale = downScale
                },
            };
        }
    }
}

