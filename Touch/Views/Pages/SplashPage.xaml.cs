using System;
using System.Numerics;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using Touch.Data;
using Touch.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Touch.Views.Pages
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class SplashPage : Page
    {
        /// <summary>
        ///     Variable to hold the splash screen object.
        /// </summary>
        private readonly SplashScreen _splash;

        private Compositor _compositor;
        private SpriteVisual _hostSprite;

        /// <summary>
        ///     Rect to store splash screen image coordinates.
        /// </summary>
        private Rect _splashImageRect;

        public SplashPage(SplashScreen splashscreen)
        {
            InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is
            // formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += (sender, args) =>
            {
                // Safely update the extended splash screen image coordinates.
                // This function will be fired in response to snapping, unsnapping, rotation, etc...
                if (_splash == null)
                    return;
                // Update the coordinates of the splash screen image.
                _splashImageRect = _splash.ImageLocation;
                PositionImage();
                PositionRing();
            };

            _splash = splashscreen;

            if (_splash == null)
                return;
            // Retrieve the window coordinates of the splash screen image.
            _splashImageRect = _splash.ImageLocation;
            // Position the extended splash screen image in the same location as the system splash screen image.
            PositionImage();
            // Optional: Add a progress ring to your splash screen to show users that content is loading
            PositionRing();
            // 模糊效果
            ApplyAcrylicAccent(MainGrid);
            MainGrid.SizeChanged += (sender, args) =>
            {
                if (_hostSprite != null)
                    _hostSprite.Size = args.NewSize.ToVector2();
            };
        }

        /// <summary>
        ///     背景模糊特性
        /// </summary>
        /// <param name="panel"></param>
        private void ApplyAcrylicAccent(FrameworkElement panel)
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _hostSprite = _compositor.CreateSpriteVisual();
            _hostSprite.Size = new Vector2((float) panel.ActualWidth, (float) panel.ActualHeight);

            ElementCompositionPreview.SetElementChildVisual(panel, _hostSprite);
            // TODO 14393 15063
            _hostSprite.Brush = _compositor.CreateHostBackdropBrush();
            var bloomEffectDesc = new ArithmeticCompositeEffect
            {
                Name = "Bloom",
                Source1Amount = 1,
                Source2Amount = 2,
                MultiplyAmount = 0,

                Source1 = new CompositionEffectSourceParameter("source"),
                Source2 = new GaussianBlurEffect
                {
                    Name = "Blur",
                    BorderMode = EffectBorderMode.Hard,
                    BlurAmount = 40,

                    Source = new BlendEffect
                    {
                        Mode = BlendEffectMode.Multiply,

                        Background = new CompositionEffectSourceParameter("source2"),
                        Foreground = new CompositionEffectSourceParameter("source2")
                    }
                }
            };

            var bloomEffectFactory = _compositor.CreateEffectFactory(bloomEffectDesc,
                new[] {"Bloom.Source2Amount", "Blur.BlurAmount"});
            var brush = bloomEffectFactory.CreateBrush();

            var backdropBrush = _compositor.CreateHostBackdropBrush();
            brush.SetSourceParameter("source", backdropBrush);
            brush.SetSourceParameter("source2", backdropBrush);

            // Setup some animations for the bloom effect
            var blurAnimation = _compositor.CreateScalarKeyFrameAnimation();
            blurAnimation.InsertKeyFrame(0, 0);
            blurAnimation.InsertKeyFrame(.5f, 2);
            blurAnimation.InsertKeyFrame(1, 0);
            blurAnimation.Duration = TimeSpan.FromMilliseconds(5000);
            blurAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            var bloomAnimation = _compositor.CreateScalarKeyFrameAnimation();
            bloomAnimation.InsertKeyFrame(0, 0);
            bloomAnimation.InsertKeyFrame(.5f, 40);
            bloomAnimation.InsertKeyFrame(1, 0);
            bloomAnimation.Duration = TimeSpan.FromMilliseconds(5000);
            bloomAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            brush.StartAnimation("Bloom.Source2Amount", blurAnimation);
            brush.StartAnimation("Blur.BlurAmount", bloomAnimation);

            _hostSprite.Brush = brush;
        }

        private void PositionImage()
        {
            ExtendedSplashImage.SetValue(Canvas.LeftProperty, _splashImageRect.X);
            ExtendedSplashImage.SetValue(Canvas.TopProperty, _splashImageRect.Y);
            ExtendedSplashImage.Height = _splashImageRect.Height;
            ExtendedSplashImage.Width = _splashImageRect.Width;
        }

        private void PositionRing()
        {
            SplashProgressRing.SetValue(Canvas.LeftProperty,
                _splashImageRect.X + _splashImageRect.Width * 0.5 - SplashProgressRing.Width * 0.5);
            SplashProgressRing.SetValue(Canvas.TopProperty,
                _splashImageRect.Y + _splashImageRect.Height + _splashImageRect.Height * 0.1);
        }

        private async void Splash_OnLoaded(object sender, RoutedEventArgs e)
        {
            // 初始化数据库
            DatabaseHelper.GetInstance();
            // 初始化图片list
            await ViewModelLocator.Instance.GalleryImageListViewModel.GetInstanceAsync();
            // 初始化回忆list
            // await MemoryListViewModel.GetInstanceAsync();
            await ViewModelLocator.Instance.MemoryListViewModel.GetInstanceAsync();

            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = rootFrame;
        }
    }
}