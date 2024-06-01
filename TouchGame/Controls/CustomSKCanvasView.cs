using SkiaSharp.Views.Maui.Controls;

namespace TouchGame
{
	public class CustomSKCanvasView : SKCanvasView
	{
		public static readonly BindableProperty ResetTouchesProperty = BindableProperty.Create(
            nameof(ResetTouches),
            typeof(bool),
            typeof(CustomSKCanvasView),
            false);

        public bool ResetTouches
        {
            set { this.SetValue(ResetTouchesProperty, value); }
            get { return (bool)this.GetValue(ResetTouchesProperty); }
        }

        //public event EventHandler ResetTouches;
	}
}

