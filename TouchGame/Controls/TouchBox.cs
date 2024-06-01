
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls.Shapes;
using SkiaSharp.Views.Maui;

namespace TouchGame
{
    public class TouchBox : Grid
    {
        private const int MAX_ALLOWED_TOUCHES = 5;

        public static readonly BindableProperty WinnersProperty = BindableProperty.Create(
            nameof(Winners),
            typeof(int),
            typeof(TouchBox),
            coerceValue: (b, n) =>
            {
                return int.Clamp((int)n, 1, 4);
            });

        public int Winners
        {
            get { return (int)GetValue(WinnersProperty); }
            set { SetValue(WinnersProperty, value); }
        }

        private static readonly BindablePropertyKey TouchesCountPropertyKey = BindableProperty.CreateReadOnly(
            nameof(TouchesCount),
            typeof(int),
            typeof(TouchBox),
            0,
            propertyChanged: (b, o, n) => ((TouchBox)b).OnTouchesCountUpdated((int)o, (int)n));

        public static readonly BindableProperty TouchesCountProperty = TouchesCountPropertyKey.BindableProperty;

        public int TouchesCount
        {
            get { return (int)GetValue(TouchesCountProperty); }
            private set { SetValue(TouchesCountPropertyKey, value); }
        }

        internal event EventHandler<SKTouchEventArgs> Touched;

        private Label _title;
        private CustomSKCanvasView _touchCanvas;
        private Dictionary<long, View> _activeSpots = new();

        private int _startCount;
        private bool _waitToFinish;
        private CancellationTokenSource _countdownCancellationToken = new CancellationTokenSource();

        public TouchBox()
		{
            Initialize();

            _title = new Label
            {
                Text = "TOUCH GAME",
                FontSize = 48,
                TextColor = Colors.White,
                FontFamily = (string)App.Current.Resources["BoldFontFamily"],
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                InputTransparent = true,
                CharacterSpacing = 1.1,
            };

            this.Add(_title);
		}

        private void OnCanvasTouch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    OnPressed(e);
                    break;

                case SKTouchAction.Moved:
                    OnMoved(e);
                    break;

                case SKTouchAction.Cancelled:
                case SKTouchAction.Released:
                    OnReleased(e);
                    break;
                    //OnCancelled(e);
                    //break;
            }
        }

        private void OnPressed(SKTouchEventArgs args)
        {
            if (_waitToFinish)
            {
                return;
            }

            TouchesCount++;

            var touch = new SKTouch(args);
            Touched?.Invoke(this, args);

            var touchSpot = CreateTouchSpot(touch);

            _activeSpots.Add(args.Id, touchSpot);

            if (TouchesCount >= Winners + 1)
            {
                _countdownCancellationToken?.Cancel();
                _countdownCancellationToken = new CancellationTokenSource();
                //Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(1),
                //    () =>
                StartCountdown(_countdownCancellationToken.Token);
            }
        }

        private void OnMoved(SKTouchEventArgs args)
        {
            if (_activeSpots.TryGetValue(args.Id, out View spot))
            {
                spot.TranslationX = args.Location.X - spot.WidthRequest / 2;
                spot.TranslationY = args.Location.Y - spot.HeightRequest / 2;
            }
        }

        private void OnReleased(SKTouchEventArgs args)
        {
            if (_waitToFinish)
            {
                return;
            }

            if (_activeSpots.TryGetValue(args.Id, out View spot))
            {
                _activeSpots.Remove(args.Id);
                this.Remove(spot);

                TouchesCount--;

                _countdownCancellationToken?.Cancel();

                if (TouchesCount >= Winners + 1 && _startCount > 0)
                {
                    _countdownCancellationToken = new CancellationTokenSource();
                    //Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(1),
                    //    () =>
                    StartCountdown(_countdownCancellationToken.Token);
                        //);
                }
            }
        }

        private void OnCancelled(SKTouchEventArgs args)
        {
            if (TouchesCount == MAX_ALLOWED_TOUCHES)
            {
                return;
            }

            OnReleased(args);
        }

        private async void StartCountdown(CancellationToken cancellationToken)
        {
            int countdown = 3;
            var label = new Label
            {
                FontSize = 50,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                InputTransparent = true
            };

            var upDownScale = ScaleBehavior.UpDownScaleBehavior(Easing.Linear, 150, 1.4, downLength: 150);
            label.Behaviors.Add(upDownScale);

            this.Add(label);

            try
            {
                _startCount++;

                while (countdown > 0)
                {
                    label.Text = countdown.ToString();
                    upDownScale.AnimateCommand.Execute(CancellationToken.None);

                    VibrationEffects.PerformHaptic(HapticFeedbackType.LongPress);

                    await Task.Delay(1000, cancellationToken);
                    countdown--;
                }

                _waitToFinish = true;
                EndGame();
            }
            catch (TaskCanceledException)
            {
                
            }
            finally
            {
                _startCount--;
                this.Remove(label);
            }
        }

        private Border CreateTouchSpot(SKTouch touch)
        {
            var newBox = new Border
            {
                HeightRequest = 130,
                WidthRequest = 130,
                StrokeShape = new RoundRectangle { CornerRadius = 65 },
                StrokeThickness = 18,
                Stroke = ColorHelper.GetRandomColor(),
                BackgroundColor = Colors.Transparent,
                InputTransparent = true,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Shadow = new Shadow { Brush = Colors.Gray, Radius = 12, Opacity = 0.3f }
            };

            newBox.TranslationX = touch.X - newBox.WidthRequest / 2;
            newBox.TranslationY = touch.Y - newBox.HeightRequest / 2;

            var upDownScale = ScaleBehavior.UpDownScaleBehavior(Easing.Linear, 200, 1.5, downLength: 200);
            newBox.Behaviors.Add(upDownScale);

            this.Add(newBox);
            VibrationEffects.PerformHaptic(HapticFeedbackType.Click);
            upDownScale.AnimateCommand.Execute(CancellationToken.None);

            return newBox;
        }

        private void Initialize()
        {
            _touchCanvas = new CustomSKCanvasView
            {
                BackgroundColor = Colors.Transparent,
                HeightRequest = Height,
                WidthRequest = Width,
                EnableTouchEvents = true,
            };

            _touchCanvas.Touch += OnCanvasTouch;

            this.Add(_touchCanvas);
        }

        private void EndGame()
        {
            VibrationEffects.Vibrate(300);

            var winners = RandomExtensions.Random(_activeSpots, Winners).ToList();

            foreach (var item in _activeSpots)
            {
                if (!winners.Contains(item))
                {
                    this.Remove(item.Value);
                }
            }

            _activeSpots.RemoveAll(p => !winners.Contains(p));

            _activeSpots.ForEach((k,v) =>
            {
                var upScale = v.Behaviors.First() as AnimationBehavior;
                upScale.AnimateCommand.Execute(CancellationToken.None);
            });

            var audioService = Application.Current.Handler.MauiContext.Services.GetService<IAudioService>();
            audioService.PlayAudio("win_sound.mp3");

            Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(3), RestartGame);
        }

        public void RestartGame()
        {
            foreach (var item in _activeSpots.Values)
            {
                this.Remove(item);
            }

            _touchCanvas.ResetTouches = true;

            _activeSpots.Clear();
            _activeSpots = new Dictionary<long, View>();
            TouchesCount = 0;
            _countdownCancellationToken = null;
            _waitToFinish = false;
        }

        private void OnTouchesCountUpdated(int oldValue, int newValue)
        {
            if (oldValue != newValue)
            {
                _title.Opacity = newValue > 0 ? 0 : 1;
            }
        }
    }

    internal class SKTouch
    {
        internal long Id { get; set; }
        internal double X { get; set; }
        internal double Y { get; set; }

        internal SKTouch(double x, double y, long id)
        {
            Id = id;
            X = x;
            Y = y;
        }

        internal SKTouch(SKTouchEventArgs args)
        {
            Id = args.Id;
            X = args.Location.X;
            Y = args.Location.Y;
        }
    }
}

