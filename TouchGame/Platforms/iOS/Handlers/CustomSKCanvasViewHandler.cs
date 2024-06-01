using SkiaSharp;
using SkiaSharp.Views.iOS;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Handlers;

namespace TouchGame
{
    public class CustomSKCanvasViewHandler : SKCanvasViewHandler
    {
        public static PropertyMapper<ISKCanvasView, CustomSKCanvasViewHandler> CustomSKCanvasMapper = new PropertyMapper<ISKCanvasView, CustomSKCanvasViewHandler>(SKCanvasViewMapper)
        {
            [nameof(ISKCanvasView.EnableTouchEvents)] = CustomEnableTouchEvents,
            [nameof(CustomSKCanvasView.ResetTouches)] = MapResetTouches,
        };

        private SKTouchHandler touchHandler;

        public CustomSKCanvasViewHandler() : base(CustomSKCanvasMapper, SKCanvasViewCommandMapper)
        {
        }

        protected override void DisconnectHandler(SKCanvasView platformView)
        {
            touchHandler?.Detach(platformView);
            touchHandler = null;

            base.DisconnectHandler(platformView);
        }

        public static void MapResetTouches(CustomSKCanvasViewHandler handler, ISKCanvasView canvasView)
        {
            if (canvasView is CustomSKCanvasView custom && custom.ResetTouches)
            {
                handler.touchHandler.ResetTouches();
                custom.ResetTouches = false;
            }
        }

        public static void CustomEnableTouchEvents(CustomSKCanvasViewHandler handler, ISKCanvasView canvasView)
        {
            handler.touchHandler ??= new SKTouchHandler(
                args => canvasView.OnTouch(args),
                (x, y) => handler.OnGetScaledCoord(x, y));
            //,
            //    handler.PlatformView);

            handler.touchHandler?.SetEnabled(handler.PlatformView, canvasView.EnableTouchEvents);
        }

        private SKPoint OnGetScaledCoord(double x, double y)
        {
            if (VirtualView?.IgnorePixelScaling == false && PlatformView != null)
            {
                var scale = PlatformView.ContentScaleFactor;

                x *= scale;
                y *= scale;
            }

            return new SKPoint((float)x, (float)y);
        }
    }
}

