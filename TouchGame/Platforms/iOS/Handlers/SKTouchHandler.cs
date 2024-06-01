using Foundation;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using UIKit;

namespace TouchGame
{
    internal class SKTouchHandler : UIGestureRecognizer
    {
        private const int MAX_ALLOWED_TOUCHES = 5;

        private readonly HashSet<long> _activeTouches = new();

        private Action<SKTouchEventArgs> _onTouchAction;
        private Func<double, double, SKPoint> _scalePixels;

        public SKTouchHandler(Action<SKTouchEventArgs> onTouchAction, Func<double, double, SKPoint> scalePixels)
        {
            _onTouchAction = onTouchAction;
            _scalePixels = scalePixels;

            DisablesUserInteraction = false;
        }

        public bool DisablesUserInteraction { get; set; }

        public void SetEnabled(UIView view, bool enableTouchEvents)
        {
            if (view != null)
            {
                if (!view.UserInteractionEnabled || DisablesUserInteraction)
                {
                    view.UserInteractionEnabled = enableTouchEvents;
                }
                if (enableTouchEvents && view.GestureRecognizers?.Contains(this) != true)
                {
                    view.AddGestureRecognizer(this);
                }
                else if (!enableTouchEvents && view.GestureRecognizers?.Contains(this) == true)
                {
                    view.RemoveGestureRecognizer(this);
                }
            }
        }

        public void Detach(UIView view)
        {
            // clean the view
            SetEnabled(view, false);

            // remove references
            _onTouchAction = null;
            _scalePixels = null;
        }

        public void ResetTouches()
        {
            _activeTouches?.Clear();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                var id = ((IntPtr)touch.Handle).ToInt64();

                if (_activeTouches.Count < MAX_ALLOWED_TOUCHES)
                {
                    _activeTouches.Add(id);

                    FireEvent(SKTouchAction.Pressed, touch, true);
                }
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                FireEvent(SKTouchAction.Moved, touch, true);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                var id = ((IntPtr)touch.Handle).ToInt64();

                if (_activeTouches.Contains(id))
                {
                    FireEvent(SKTouchAction.Released, touch, false);

                    _activeTouches.Remove(id);
                }
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                var id = ((IntPtr)touch.Handle).ToInt64();

                if (_activeTouches.Contains(id) && _activeTouches.Count < MAX_ALLOWED_TOUCHES)
                {
                    _activeTouches.Remove(id);

                    FireEvent(SKTouchAction.Cancelled, touch, false);
                }
            }
        }

        private bool FireEvent(SKTouchAction actionType, UITouch touch, bool inContact)
        {
            if (_onTouchAction == null || _scalePixels == null)
            {
                return false;
            }

            var id = ((IntPtr)touch.Handle).ToInt64();

            var cgPoint = touch.LocationInView(View);
            var point = new SKPoint((float)cgPoint.X, (float)cgPoint.Y);

            var args = new SKTouchEventArgs(id, actionType, point, inContact);
            _onTouchAction(args);

            return args.Handled;
        }
    }
}