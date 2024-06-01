namespace TouchGame
{
	public class ColorHelper
	{
		public static Color GetRandomColor()
		{
            Random random = new Random();
            byte[] rgb = new byte[3];
            random.NextBytes(rgb);

            return Color.FromRgb(rgb[0], rgb[1], rgb[2]);
        }

        public static List<Brush> GetBackgroundBrushes()
        {
            return new List<Brush>
            {
                new LinearGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Colors.Yellow, 0.0f),
                        new GradientStop(Colors.Orange, 0.2f),
                        new GradientStop(Colors.DarkOrange, 0.4f),
                        new GradientStop(Colors.OrangeRed, 0.7f),
                        new GradientStop(Colors.Red, 0.9f),
                        new GradientStop(Colors.DarkRed, 1.0f),
                    },
                    new Point(0,0), new Point(0,1)),
                new LinearGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Colors.LightCyan, 0.0f),
                        new GradientStop(Colors.Cyan, 0.2f),
                        new GradientStop(Colors.DeepSkyBlue, 0.4f),
                        new GradientStop(Colors.DodgerBlue, 0.6f),
                        new GradientStop(Colors.MediumBlue, 0.8f),
                        new GradientStop(Colors.DarkBlue, 1.0f),
                    },
                    new Point(0,0), new Point(0,1)),
                new LinearGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Colors.HotPink, 0.0f),
                        new GradientStop(Colors.DeepPink, 0.3f),
                        new GradientStop(Colors.MediumVioletRed, 0.5f),
                        new GradientStop(Colors.DarkMagenta, 0.7f),
                        new GradientStop(Colors.Purple, 1.0f)
                    },
                    new Point(0,0), new Point(0,1)),
                new LinearGradientBrush(
                    new GradientStopCollection
                    {
                        new GradientStop(Colors.GreenYellow, 0.0f),
                        new GradientStop(Colors.YellowGreen, 0.2f),
                        new GradientStop(Colors.MediumSeaGreen, 0.4f),
                        new GradientStop(Colors.SeaGreen, 0.6f),
                        new GradientStop(Colors.ForestGreen, 0.8f),
                        new GradientStop(Colors.Green, 1.0f),
                    },
                    new Point(0,0), new Point(0,1)),
            };
        }
	}
}

