using CommunityToolkit.Maui.Animations;

namespace TouchGame
{
	public class UpDownScaleAnimation : BaseAnimation
	{
		public double UpScale { get; set; }
        public double? DownScale { get; set; }

        public uint? DownLength { get; set; }

        public override async Task Animate(VisualElement view, CancellationToken token = default)
        {
            await view.ScaleTo(UpScale, Length, Easing);
            await view.ScaleTo(DownScale ?? 1, DownLength ?? Length, Easing);
        }
    }
}

