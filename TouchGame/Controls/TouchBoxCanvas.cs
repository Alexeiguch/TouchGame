using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace TouchGame
{
	internal class TouchBoxCanvas : SKCanvasView
	{
        private SKCanvas _canvas;
        private SKRect _drawRect;
        private SKImageInfo _info;

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            _canvas = e.Surface.Canvas;
            _canvas.Clear();

            _info = e.Info;
            System.Diagnostics.Debug.WriteLine(_info);

            _drawRect = new SKRect(0, 0, _info.Width, _info.Height);
        }

        private void DrawSpotTouch()
        {
            using var basePath = new SKPath();

            basePath.AddRect(_drawRect);

            _canvas.DrawPath(basePath, new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Colors.Red.ToSKColor(),
                IsAntialias = true
            });
        }
    }
}